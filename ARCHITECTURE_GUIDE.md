# Project Architecture Guide - Job Management API

## Overview
This document explains the proper layered architecture for your .NET 10 CRUD API with authentication.

---

## Layer Responsibilities

### 1. **Controller Layer** ✅
**File**: `JobsController.cs`

**Responsibilities:**
- Accept HTTP requests
- Validate incoming data (ModelState)
- Extract authentication context (User ID from JWT)
- Call appropriate service methods
- Return proper HTTP responses with status codes
- Handle exceptions and return appropriate error messages

**What NOT to do:**
- ❌ Map DTO to Entity (move to Service)
- ❌ Database queries (move to Repository)
- ❌ Complex business logic (move to Service)

**Example:**
```csharp
[HttpPost("Create")]
public async Task<IActionResult> Create([FromBody] JobDto dto)
{
    // ✅ Validate input
    if (!ModelState.IsValid)
        return BadRequest(ModelState);

    // ✅ Get auth context
    var userId = GetCurrentUserId();

    // ✅ Call service (service handles mapping & logic)
    var created = await _service.CreateAsync(dto);

    // ✅ Return appropriate response
    return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
}
```

---

### 2. **Service Layer** ✅
**File**: `jobServices.cs`

**Responsibilities:**
- Map DTO → Entity (conversion logic)
- Map Entity → DTO (if needed for responses)
- Validate business rules
- Orchestrate repository calls
- Handle business logic
- Add logging for auditing

**What NOT to do:**
- ❌ Return HTTP responses
- ❌ Handle HTTP status codes
- ❌ Direct database operations (use repository)

**Example:**
```csharp
public async Task<JobEntity> CreateAsync(JobDto dto)
{
    // ✅ Validate
    if (dto is null)
        throw new ArgumentNullException(nameof(dto));

    // ✅ Map DTO to Entity
    var job = MapToEntity(dto);

    // ✅ Call repository
    return await _repo.CreateAsync(job);
}

private JobEntity MapToEntity(JobDto dto)
{
    return new JobEntity
    {
        Division = dto.Division,
        SchoolId = dto.SchoolId,
        // ... etc
    };
}
```

---

### 3. **Repository Layer**
**File**: `IJobRepository` / `JobRepository`

**Responsibilities:**
- Execute database queries
- Handle DbContext operations
- CRUD operations only
- No business logic

---

## Authentication Flow ✅

### JWT Token Flow:
1. User logs in → AuthController generates JWT token
2. Client includes token: `Authorization: Bearer {token}`
3. `JwtAuthenticationHandler` validates token
4. User claims extracted into `User` principal
5. Controller accesses user via `User.FindFirst(ClaimTypes.NameIdentifier)`

### Implementation:
```csharp
// In Controller
private int GetCurrentUserId()
{
    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
    return userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId) 
        ? userId 
        : 0;
}

// Usage
[HttpPost("Create")]
[Authorize]  // ✅ Enforces authentication
public async Task<IActionResult> Create([FromBody] JobDto dto)
{
    var userId = GetCurrentUserId();  // ✅ Get authenticated user
    // ... proceed with creation
}
```

---

## Error Handling Strategy ✅

### Layered Error Handling:

```
Controller (HTTP Responses)
    ↓
Service (Business Exceptions)
    ↓
Repository (Data Access Exceptions)
```

### Examples:

**Service Layer:**
```csharp
public async Task<JobEntity?> UpdateAsync(int id, JobDto dto)
{
    if (dto is null)
        throw new ArgumentNullException(nameof(dto));

    var existing = await _repo.GetByIdAsync(id);
    if (existing is null)
        return null;  // Service returns null for not found

    UpdateEntityFromDto(existing, dto);
    return await _repo.UpdateAsync(existing);
}
```

**Controller Layer:**
```csharp
[HttpPut("Update/{id}")]
[Authorize]
public async Task<IActionResult> Update(int id, [FromBody] JobDto dto)
{
    try
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updated = await _service.UpdateAsync(id, dto);
        if (updated is null)
            return NotFound("Job not found");  // ✅ Proper HTTP response

        return Ok(updated);
    }
    catch (ArgumentNullException ex)
    {
        return BadRequest(ex.Message);
    }
    catch (Exception ex)
    {
        _logger.LogError($"Error: {ex.Message}");
        return StatusCode(500, "An error occurred");
    }
}
```

---

## Logging & Auditing ✅

### Implementation:
```csharp
// Inject ILogger
public JobsController(IJobService service, ILogger<JobsController> logger)
{
    _logger = logger;
}

// Log user actions with authentication context
public async Task<IActionResult> Create([FromBody] JobDto dto)
{
    var userId = GetCurrentUserId();
    _logger.LogInformation($"User {userId} creating new job");

    var created = await _service.CreateAsync(dto);

    _logger.LogInformation($"User {userId} successfully created job {created.Id}");
    return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
}
```

---

## Data Flow Diagram

```
┌─────────────────┐
│    HTTP Client  │
│ (with JWT Token)│
└────────┬────────┘
         │ 1. POST /api/job/Create
         │ Authorization: Bearer {token}
         ▼
┌─────────────────────────────────────┐
│      JwtAuthenticationHandler       │
│    (Validates token, sets User)     │
└────────┬────────────────────────────┘
         │ 2. Token valid → User principal set
         ▼
┌─────────────────────────────────────┐
│         JobsController              │
│  [Authorize] - Verifies auth        │
│  Validates ModelState               │
│  Gets user ID from JWT              │
│  Calls service                      │
└────────┬────────────────────────────┘
         │ 3. JobDto passed to service
         ▼
┌─────────────────────────────────────┐
│           JobService                │
│  Maps DTO → Entity                  │
│  Validates business rules           │
│  Calls repository                   │
└────────┬────────────────────────────┘
         │ 4. JobEntity passed to repo
         ▼
┌─────────────────────────────────────┐
│         JobRepository               │
│  Saves to database via DbContext    │
└────────┬────────────────────────────┘
         │ 5. Returns created entity
         ▼
┌────────────────────────────────────────────┐
│  Service returns JobEntity to Controller   │
└────────┬─────────────────────────────────────┘
         │ 6. Controller returns 201 Created
         ▼
     HTTP 201 Created
    + Location header
    + Created Job object
```

---

## Summary: Before vs After

### ❌ BEFORE (Your Original Code)
```csharp
// Controller
[HttpPost("Create")]
public async Task<IActionResult> Create([FromBody] JobDto dto)
{
    // Mapping logic in controller
    var job = new JobEntity
    {
        Division = dto.Division,
        SchoolId = dto.SchoolId,
        // ... many properties
    };

    var created = await _service.CreateAsync(job);
    return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
}
```

**Issues:**
- Mapping logic scattered in controller
- No separation of concerns
- Hard to test
- Authentication context not captured

### ✅ AFTER (Refactored Code)
```csharp
// Controller
[HttpPost("Create")]
[Authorize]
public async Task<IActionResult> Create([FromBody] JobDto dto)
{
    var userId = GetCurrentUserId();  // Auth context
    _logger.LogInformation($"User {userId} creating job");

    var created = await _service.CreateAsync(dto);  // Service handles mapping

    return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
}
```

**Benefits:**
- ✅ Clean separation of concerns
- ✅ Service handles all mapping & logic
- ✅ Authentication context captured
- ✅ Easy to test
- ✅ Proper error handling
- ✅ Audit logging

---

## Best Practices Implemented

1. **Separation of Concerns**: Each layer has a single responsibility
2. **Authentication**: JWT tokens validated, user context extracted
3. **Logging**: All user actions logged with authentication context
4. **Error Handling**: Graceful exception handling at each layer
5. **Validation**: Input validation at controller level
6. **Null Checks**: Defensive programming throughout
7. **Async/Await**: Proper async operations for scalability
8. **ILogger Injection**: Structured logging via DI

---

## Next Steps (Optional Improvements)

1. **AutoMapper**: Use for complex DTOs
   ```csharp
   services.AddAutoMapper(typeof(Program));
   ```

2. **FluentValidation**: Advanced DTO validation
   ```csharp
   services.AddFluentValidation();
   ```

3. **Global Exception Handling**: Middleware for centralized error handling
   ```csharp
   app.UseMiddleware<ExceptionHandlingMiddleware>();
   ```

4. **Audit Trail**: Database tracking of user actions
5. **Role-Based Authorization**: `[Authorize(Roles = "Admin")]`

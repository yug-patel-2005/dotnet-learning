using CRUDproject.Models;
using CRUDproject.Reposetries.JobRepository.Interface;
using Microsoft.EntityFrameworkCore;

namespace CRUDproject.Reposetries.JobRepository
{
    public class JobRepository : IJobRepository
    {
        private readonly AppDbContext _db;

        public JobRepository(AppDbContext db)
        {
            _db = db;
        }

        // CREATE: Just save as usual (Service will set the CreatedBy ID)
        public async Task<JobEntity> CreateAsync(JobEntity job)
        {
            await _db.Jobs.AddAsync(job);
            await _db.SaveChangesAsync();
            return job;
        }

        // READ: Only return jobs belonging to this specific user
        public async Task<IEnumerable<JobEntity>> GetAllAsync(int userId)
        {
            return await _db.Jobs
                .Where(j => j.CreatedBy == userId) // THE FILTER
                .AsNoTracking()
                .ToListAsync();
        }

        // READ ONE: Ensure the job exists AND belongs to the user
        public async Task<JobEntity?> GetByIdAsync(int id, int userId)
        {
            return await _db.Jobs
                .FirstOrDefaultAsync(j => j.Id == id && j.CreatedBy == userId);
        }

        // UPDATE: Find by ID and Owner before allowing changes
        public async Task<JobEntity?> UpdateAsync(JobEntity job, int userId)
        {
            // Notice we check BOTH ID and CreatedBy
            var existing = await _db.Jobs
                .FirstOrDefaultAsync(j => j.Id == job.Id && j.CreatedBy == userId);

            if (existing == null) return null;

            _db.Entry(existing).CurrentValues.SetValues(job);
            await _db.SaveChangesAsync();
            return existing;
        }

        // DELETE: Only allow if it belongs to the user
        public async Task<JobEntity?> DeleteAsync(int id, int userId)
        {
            var existing = await _db.Jobs
                .FirstOrDefaultAsync(j => j.Id == id && j.CreatedBy == userId);

            if (existing == null) return null;

            _db.Jobs.Remove(existing);
            await _db.SaveChangesAsync();
            return existing;
        }
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using CRUDproject.Models;
using CRUDproject.Models.AuthUser;
using CRUDproject.Models.UserRoles;
using CRUDproject.Reposetries.UserRoleRepo.Interface;
using Microsoft.EntityFrameworkCore;

namespace CRUDproject.Reposetries.UserRoleRepo
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly AppDbContext _context;

        public UserRoleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllUserRolesAsync()
        {
            return await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .ToListAsync();
        }

        public async Task<User?> GetUserRolesAsync(int userId)
        {
            return await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<bool> AssignRoleAsync(int userId, int roleId)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
            if (!userExists) return false;

            var roleExists = await _context.Roles.AnyAsync(r => r.RoleId == roleId);
            if (!roleExists) return false;

            var existingRole = await _context.UserRoles
                .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);
            
            if (existingRole != null) return true; // Already assigned

            _context.UserRoles.Add(new UserRole { UserId = userId, RoleId = roleId });
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveRoleAsync(int userId, int roleId)
        {
            var userRole = await _context.UserRoles
                .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);
            
            if (userRole == null) return false;

            _context.UserRoles.Remove(userRole);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

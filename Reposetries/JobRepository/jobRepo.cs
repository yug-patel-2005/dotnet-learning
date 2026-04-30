using CRUDproject.Enums;
using CRUDproject.Models;
using CRUDproject.Reposetries.JobRepository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CRUDproject.Reposetries.JobRepository
{
    public class JobRepository : IJobRepository
    {
        private readonly AppDbContext _db;

        public JobRepository(AppDbContext db)
        {
            _db = db;
        }

       
        public async Task<JobEntity> CreateAsync(JobEntity job)
        {
            await _db.Jobs.AddAsync(job);
            await _db.SaveChangesAsync();
            return job;
        }

       
        public async Task<IEnumerable<JobEntity>> GetAllAsync(int userId,int roleId)
        {
            if (roleId == (int)UserRole.Admin) 
            {
                return await _db.Jobs.AsNoTracking().ToListAsync();
            }
            return await _db.Jobs
                 .Where(j => j.UserId == userId)
                 .AsNoTracking()
                 .ToListAsync();
        }

       
        public async Task<JobEntity?> GetByIdAsync(int id, int userId, int roleId)
        {

            if (roleId == (int)UserRole.Admin)
            {
                return await _db.Jobs.FirstOrDefaultAsync(j => j.Id == id);
            }

            return await _db.Jobs
                  .FirstOrDefaultAsync(j => j.Id == id && j.UserId == userId);
        }

       
        public async Task<JobEntity?> UpdateAsync(JobEntity job, int userId, int roleId)
        {
            
            var existing = await GetByIdAsync(job.Id, userId, roleId);

            if (existing == null) return null;

            _db.Entry(existing).CurrentValues.SetValues(job);
            await _db.SaveChangesAsync();
            return existing;
        }

        public async Task<JobEntity?> DeleteAsync(int id, int userId, int roleId)
        {

            if (roleId != (int)UserRole.Admin)
            {
                return null; 
            }
            var existing = await GetByIdAsync(id, userId, roleId);

            if (existing == null) return null;

            _db.Jobs.Remove(existing);
            await _db.SaveChangesAsync();
            return existing;
        }
        public async Task<IEnumerable<JobEntity>> GetByDateAsync(DateTime date)
        {
            return await _db.Jobs
        .AsNoTracking()
        .Where(j => j.ShootDate.HasValue && j.ShootDate.Value.Date == date.Date)
        .ToListAsync();
        }
    }
}

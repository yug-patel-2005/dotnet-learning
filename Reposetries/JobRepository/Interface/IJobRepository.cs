using CRUDproject.Models;

namespace CRUDproject.Reposetries.JobRepository.Interface
{
    public interface IJobRepository
    {
        Task<JobEntity> CreateAsync(JobEntity job);
        Task<IEnumerable<JobEntity>> GetAllAsync(int userId, int roleId);
        Task<JobEntity?> GetByIdAsync(int id, int userId, int roleId);
        Task<JobEntity?> UpdateAsync(JobEntity job, int userId, int roleId);
        Task<JobEntity?> DeleteAsync(int id, int userId, int roleId);
        Task<IEnumerable<JobEntity>> GetByDateAsync(int userId, DateTime date);
        Task<bool> AssignUserAsync(int jobId, int userId);
    }
}
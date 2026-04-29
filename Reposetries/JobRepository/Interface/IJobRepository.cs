using CRUDproject.Models;

namespace CRUDproject.Reposetries.JobRepository.Interface
{
    public interface IJobRepository
    {
       
        Task<JobEntity> CreateAsync(JobEntity job);
        Task<JobEntity?> UpdateAsync(JobEntity job, int userId);
        Task<JobEntity?> DeleteAsync(int id, int userId);
        Task<JobEntity?> GetByIdAsync(int id, int userId);
        Task<IEnumerable<JobEntity>> GetAllAsync(int userId); 
    }
}

using CRUDproject.Dtos.JobDto;
using CRUDproject.Models;

namespace CRUDproject.Services.JobService.Interface
{
    public interface IJobService
    {

        Task<JobEntity> CreateAsync(JobDto dto, int currentUserId);
        Task<JobEntity?> UpdateAsync(int id, JobDto dto, int currentUserId, int roleId);
        Task<JobEntity?> DeleteAsync(int id, int currentUserId, int roleId);
        Task<JobEntity?> GetByIdAsync(int id, int currentUserId, int roleId);
        Task<IEnumerable<JobEntity>> GetAllAsync(int currentUserId, int roleId);
        Task<IEnumerable<JobEntity>> GetJobsByDateAsync(DateTime date);
       
    }
}
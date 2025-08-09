using BackendDotNet.Models;

namespace BackendDotNet.Repositories
{
    public interface IBloodRequestRepository
    {
        Task<BloodRequest?> GetByIdAsync(long id);
        Task<List<BloodRequest>> GetAllAsync();
        Task<List<BloodRequest>> GetByHospitalIdAsync(long hospitalId);
        Task<List<BloodRequest>> GetByHospitalUserIdAsync(long hospitalUserId);
        Task<List<BloodRequest>> GetByStatusAsync(string status);
        Task<long> CountAsync();
        Task<long> CountByStatusAsync(string status);
        Task<BloodRequest> AddAsync(BloodRequest bloodRequest);
        Task<BloodRequest> CreateAsync(BloodRequest bloodRequest);
        Task<BloodRequest> UpdateAsync(BloodRequest bloodRequest);
        Task<bool> DeleteAsync(long id);
    }
}

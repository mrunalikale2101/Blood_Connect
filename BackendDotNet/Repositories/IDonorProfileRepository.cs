using BackendDotNet.Models;

namespace BackendDotNet.Repositories
{
    public interface IDonorProfileRepository
    {
        Task<DonorProfile?> GetByIdAsync(long id);
        Task<DonorProfile?> GetByUserIdAsync(long userId);
        Task<IEnumerable<DonorProfile>> GetAllAsync();
        Task<DonorProfile> AddAsync(DonorProfile donorProfile);
        Task<DonorProfile> UpdateAsync(DonorProfile donorProfile);
        Task<bool> DeleteAsync(long userId);
        Task<bool> ExistsByUserIdAsync(long userId);
        Task<long> CountAsync();
        Task<List<DonorProfile>> GetAllWithUserAsync();
        Task<IEnumerable<DonorProfile>> GetEligibleDonorsAsync();
    }
}

using BackendDotNet.Models;

namespace BackendDotNet.Repositories
{
    public interface IHospitalProfileRepository
    {
        Task<HospitalProfile?> GetByIdAsync(long id);
        Task<HospitalProfile?> GetByUserIdAsync(long userId);
        Task<IEnumerable<HospitalProfile>> GetAllAsync();
        Task<HospitalProfile> AddAsync(HospitalProfile hospitalProfile);
        Task<HospitalProfile> UpdateAsync(HospitalProfile hospitalProfile);
        Task<bool> DeleteAsync(long userId);
        Task<bool> ExistsByUserIdAsync(long userId);
        Task<long> CountAsync();
        Task<IEnumerable<HospitalProfile>> GetVerifiedHospitalsAsync();
    }
}

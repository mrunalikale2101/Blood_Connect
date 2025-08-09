using BackendDotNet.Models;

namespace BackendDotNet.Repositories
{
    public interface IDonationRecordRepository
    {
        Task<DonationRecord> CreateAsync(DonationRecord record);
        Task<DonationRecord?> GetByIdAsync(long recordId);
        Task<List<DonationRecord>> GetByDonorIdAsync(long donorId);
        Task<List<DonationRecord>> GetAllAsync();
        Task<List<DonationRecord>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}

using BackendDotNet.Data;
using BackendDotNet.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendDotNet.Repositories
{
    public class DonationRecordRepository : IDonationRecordRepository
    {
        private readonly ApplicationDbContext _context;

        public DonationRecordRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DonationRecord> CreateAsync(DonationRecord record)
        {
            _context.DonationRecords.Add(record);
            await _context.SaveChangesAsync();
            return record;
        }

        public async Task<DonationRecord?> GetByIdAsync(long recordId)
        {
            return await _context.DonationRecords
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.RecordId == recordId);
        }

        public async Task<List<DonationRecord>> GetByDonorIdAsync(long donorId)
        {
            return await _context.DonationRecords
                .AsNoTracking()
                .Where(r => r.DonorUserId == donorId)
                .OrderByDescending(r => r.DonationDate)
                .ToListAsync();
        }

        public async Task<List<DonationRecord>> GetAllAsync()
        {
            return await _context.DonationRecords
                .AsNoTracking()
                .OrderByDescending(r => r.DonationDate)
                .ToListAsync();
        }

        public async Task<List<DonationRecord>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.DonationRecords
                .AsNoTracking()
                .Where(r => r.DonationDate >= startDate && r.DonationDate <= endDate)
                .OrderByDescending(r => r.DonationDate)
                .ToListAsync();
        }
    }
}

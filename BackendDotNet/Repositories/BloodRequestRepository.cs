using BackendDotNet.Data;
using BackendDotNet.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendDotNet.Repositories
{
    public class BloodRequestRepository : IBloodRequestRepository
    {
        private readonly ApplicationDbContext _context;

        public BloodRequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BloodRequest?> GetByIdAsync(long id)
        {
            return await _context.BloodRequests
                .Include(br => br.Hospital)
                .ThenInclude(h => h.HospitalProfile)
                .FirstOrDefaultAsync(br => br.RequestId == id);
        }

        public async Task<List<BloodRequest>> GetAllAsync()
        {
            return await _context.BloodRequests
                .Include(br => br.Hospital)
                .ThenInclude(h => h.HospitalProfile)
                .OrderByDescending(br => br.RequestDate)
                .ToListAsync();
        }

        public async Task<List<BloodRequest>> GetByHospitalUserIdAsync(long hospitalId)
        {
            return await _context.BloodRequests
                .Include(br => br.Hospital)
                .ThenInclude(h => h.HospitalProfile)
                .Where(br => br.HospitalId == hospitalId)
                .OrderByDescending(br => br.RequestDate)
                .ToListAsync();
        }

        public async Task<List<BloodRequest>> GetByStatusAsync(string status)
        {
            return await _context.BloodRequests
                .Include(br => br.Hospital)
                .ThenInclude(h => h.HospitalProfile)
                .Where(br => br.Status == status)
                .OrderByDescending(br => br.RequestDate)
                .ToListAsync();
        }

        public async Task<BloodRequest> AddAsync(BloodRequest bloodRequest)
        {
            _context.BloodRequests.Add(bloodRequest);
            await _context.SaveChangesAsync();
            return bloodRequest;
        }

        public async Task<BloodRequest> UpdateAsync(BloodRequest bloodRequest)
        {
            _context.BloodRequests.Update(bloodRequest);
            await _context.SaveChangesAsync();
            return bloodRequest;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var bloodRequest = await _context.BloodRequests.FindAsync(id);
            if (bloodRequest != null)
            {
                _context.BloodRequests.Remove(bloodRequest);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<long> CountAsync()
        {
            return await _context.BloodRequests.CountAsync();
        }

        public async Task<long> CountByStatusAsync(string status)
        {
            return await _context.BloodRequests
                .Where(br => br.Status == status)
                .CountAsync();
        }

        public async Task<List<BloodRequest>> GetByHospitalIdAsync(long hospitalId)
        {
            return await _context.BloodRequests
                .Include(br => br.Hospital)
                .ThenInclude(h => h.HospitalProfile)
                .Where(br => br.HospitalId == hospitalId)
                .OrderByDescending(br => br.RequestDate)
                .ToListAsync();
        }
    }
}

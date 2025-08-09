using BackendDotNet.Data;
using BackendDotNet.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendDotNet.Repositories
{
    public class HospitalProfileRepository : IHospitalProfileRepository
    {
        private readonly ApplicationDbContext _context;

        public HospitalProfileRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<HospitalProfile?> GetByIdAsync(long id)
        {
            return await _context.HospitalProfiles
                .Include(hp => hp.User)
                .FirstOrDefaultAsync(hp => hp.ProfileId == id);
        }

        public async Task<HospitalProfile?> GetByUserIdAsync(long userId)
        {
            return await _context.HospitalProfiles
                .Include(hp => hp.User)
                .FirstOrDefaultAsync(hp => hp.UserId == userId);
        }

        public async Task<IEnumerable<HospitalProfile>> GetAllAsync()
        {
            return await _context.HospitalProfiles
                .Include(hp => hp.User)
                .ToListAsync();
        }

        public async Task<HospitalProfile> AddAsync(HospitalProfile hospitalProfile)
        {
            _context.HospitalProfiles.Add(hospitalProfile);
            await _context.SaveChangesAsync();
            return hospitalProfile;
        }

        public async Task<HospitalProfile> UpdateAsync(HospitalProfile hospitalProfile)
        {
            _context.HospitalProfiles.Update(hospitalProfile);
            await _context.SaveChangesAsync();
            return hospitalProfile;
        }

        public async Task<bool> DeleteAsync(long userId)
        {
            var hospitalProfile = await GetByUserIdAsync(userId);
            if (hospitalProfile != null)
            {
                _context.HospitalProfiles.Remove(hospitalProfile);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> ExistsByUserIdAsync(long userId)
        {
            return await _context.HospitalProfiles.AnyAsync(hp => hp.UserId == userId);
        }

        public async Task<IEnumerable<HospitalProfile>> GetVerifiedHospitalsAsync()
        {
            return await _context.HospitalProfiles
                .Include(hp => hp.User)
                .Where(hp => hp.IsVerified)
                .ToListAsync();
        }

        public async Task<long> CountAsync()
        {
            return await _context.HospitalProfiles.CountAsync();
        }
    }
}

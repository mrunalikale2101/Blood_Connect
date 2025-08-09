using BackendDotNet.Data;
using BackendDotNet.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendDotNet.Repositories
{
    public class DonorProfileRepository : IDonorProfileRepository
    {
        private readonly ApplicationDbContext _context;

        public DonorProfileRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DonorProfile?> GetByIdAsync(long id)
        {
            return await _context.DonorProfiles
                .Include(dp => dp.User)
                .FirstOrDefaultAsync(dp => dp.ProfileId == id);
        }

        public async Task<DonorProfile?> GetByUserIdAsync(long userId)
        {
            return await _context.DonorProfiles
                .Include(dp => dp.User)
                .FirstOrDefaultAsync(dp => dp.UserId == userId);
        }

        public async Task<IEnumerable<DonorProfile>> GetAllAsync()
        {
            return await _context.DonorProfiles
                .Include(dp => dp.User)
                .ToListAsync();
        }

        public async Task<DonorProfile> AddAsync(DonorProfile donorProfile)
        {
            _context.DonorProfiles.Add(donorProfile);
            await _context.SaveChangesAsync();
            return donorProfile;
        }

        public async Task<DonorProfile> UpdateAsync(DonorProfile donorProfile)
        {
            _context.DonorProfiles.Update(donorProfile);
            await _context.SaveChangesAsync();
            return donorProfile;
        }

        public async Task<bool> DeleteAsync(long userId)
        {
            var donorProfile = await GetByUserIdAsync(userId);
            if (donorProfile != null)
            {
                _context.DonorProfiles.Remove(donorProfile);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> ExistsByUserIdAsync(long userId)
        {
            return await _context.DonorProfiles.AnyAsync(dp => dp.UserId == userId);
        }

        public async Task<IEnumerable<DonorProfile>> GetEligibleDonorsAsync()
        {
            return await _context.DonorProfiles
                .Include(dp => dp.User)
                .Where(dp => dp.IsEligible)
                .ToListAsync();
        }

        public async Task<long> CountAsync()
        {
            return await _context.DonorProfiles.CountAsync();
        }

        public async Task<List<DonorProfile>> GetAllWithUserAsync()
        {
            return await _context.DonorProfiles
                .Include(dp => dp.User)
                .ToListAsync();
        }
    }
}

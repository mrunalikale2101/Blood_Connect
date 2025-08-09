using BackendDotNet.Data;
using BackendDotNet.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendDotNet.Repositories
{
    public class BloodInventoryRepository : IBloodInventoryRepository
    {
        private readonly ApplicationDbContext _context;

        public BloodInventoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BloodInventory?> GetByIdAsync(int id)
        {
            return await _context.BloodInventories.FindAsync(id);
        }

        public async Task<BloodInventory?> GetByBloodGroupAsync(string bloodGroup)
        {
            return await _context.BloodInventories
                .FirstOrDefaultAsync(bi => bi.BloodGroup == bloodGroup);
        }

        public async Task<List<BloodInventory>> GetAllAsync()
        {
            return await _context.BloodInventories.ToListAsync();
        }

        public async Task<BloodInventory> AddAsync(BloodInventory bloodInventory)
        {
            _context.BloodInventories.Add(bloodInventory);
            await _context.SaveChangesAsync();
            return bloodInventory;
        }

        public async Task<BloodInventory> UpdateAsync(BloodInventory bloodInventory)
        {
            _context.BloodInventories.Update(bloodInventory);
            await _context.SaveChangesAsync();
            return bloodInventory;
        }

        public async Task DeleteAsync(int id)
        {
            var bloodInventory = await _context.BloodInventories.FindAsync(id);
            if (bloodInventory != null)
            {
                _context.BloodInventories.Remove(bloodInventory);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetTotalUnitsAsync()
        {
            return await _context.BloodInventories.SumAsync(b => b.Units);
        }

        public async Task<long> CountAsync()
        {
            return await _context.BloodInventories.CountAsync();
        }

        public async Task<bool> DeleteAsync(long inventoryId)
        {
            var inventory = await GetByIdAsync((int)inventoryId);
            if (inventory == null) return false;
            
            _context.BloodInventories.Remove(inventory);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

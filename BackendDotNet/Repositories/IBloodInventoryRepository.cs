using BackendDotNet.Models;

namespace BackendDotNet.Repositories
{
    public interface IBloodInventoryRepository
    {
        Task<BloodInventory?> GetByIdAsync(int id);
        Task<BloodInventory?> GetByBloodGroupAsync(string bloodGroup);
        Task<List<BloodInventory>> GetAllAsync();
        Task<BloodInventory> AddAsync(BloodInventory bloodInventory);
        Task<BloodInventory> UpdateAsync(BloodInventory bloodInventory);
        Task<bool> DeleteAsync(long inventoryId);
        Task<long> CountAsync();
        Task<int> GetTotalUnitsAsync();
    }
}

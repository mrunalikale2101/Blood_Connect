using BackendDotNet.Models;

namespace BackendDotNet.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(long id);
        Task<User?> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> AddAsync(User user);
        Task<User> UpdateAsync(User user);
        Task DeleteAsync(long id);
        Task<long> CountByRoleNameAsync(string roleName);
        Task<bool> ExistsByEmailAsync(string email);
    }
}

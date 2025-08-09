using BackendDotNet.Models;

namespace BackendDotNet.Repositories
{
    public interface IContactMessageRepository
    {
        Task<ContactMessage> SaveAsync(ContactMessage contactMessage);
        Task<List<ContactMessage>> GetAllAsync();
        Task<ContactMessage?> GetByIdAsync(long id);
        Task<bool> MarkAsReadAsync(long id);
        Task<long> CountUnreadAsync();
    }
}

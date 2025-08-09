using BackendDotNet.Data;
using BackendDotNet.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendDotNet.Repositories
{
    public class ContactMessageRepository : IContactMessageRepository
    {
        private readonly ApplicationDbContext _context;

        public ContactMessageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ContactMessage> SaveAsync(ContactMessage contactMessage)
        {
            contactMessage.CreatedAt = DateTime.UtcNow;
            contactMessage.UpdatedAt = DateTime.UtcNow;
            
            _context.ContactMessages.Add(contactMessage);
            await _context.SaveChangesAsync();
            return contactMessage;
        }

        public async Task<List<ContactMessage>> GetAllAsync()
        {
            return await _context.ContactMessages
                .OrderByDescending(cm => cm.CreatedAt)
                .ToListAsync();
        }

        public async Task<ContactMessage?> GetByIdAsync(long id)
        {
            return await _context.ContactMessages.FindAsync(id);
        }

        public async Task<bool> MarkAsReadAsync(long id)
        {
            var contactMessage = await _context.ContactMessages.FindAsync(id);
            if (contactMessage != null)
            {
                contactMessage.IsRead = true;
                contactMessage.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<long> CountUnreadAsync()
        {
            return await _context.ContactMessages
                .Where(cm => !cm.IsRead)
                .CountAsync();
        }
    }
}

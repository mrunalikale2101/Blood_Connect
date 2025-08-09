using BackendDotNet.DTOs.Request;
using BackendDotNet.Models;
using BackendDotNet.Repositories;

namespace BackendDotNet.Services
{
    public class PublicService : IPublicService
    {
        private readonly IContactMessageRepository _contactMessageRepository;

        public PublicService(IContactMessageRepository contactMessageRepository)
        {
            _contactMessageRepository = contactMessageRepository;
        }

        public async Task<string> SaveContactMessageAsync(ContactMessageRequestDto requestDto)
        {
            var contactMessage = new ContactMessage
            {
                Name = requestDto.Name,
                Email = requestDto.Email,
                Message = requestDto.Message
            };

            await _contactMessageRepository.SaveAsync(contactMessage);
            return "Your message has been received. Thank you!";
        }
    }
}

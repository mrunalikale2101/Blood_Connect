using BackendDotNet.DTOs.Request;

namespace BackendDotNet.Services
{
    public interface IPublicService
    {
        Task<string> SaveContactMessageAsync(ContactMessageRequestDto requestDto);
    }
}

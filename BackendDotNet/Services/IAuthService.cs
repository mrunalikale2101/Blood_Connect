using BackendDotNet.DTOs.Request;
using BackendDotNet.DTOs.Response;

namespace BackendDotNet.Services
{
    public interface IAuthService
    {
        Task<string> RegisterDonorAsync(DonorRegisterRequest registerRequest);
        Task<string> RegisterHospitalAsync(HospitalRegisterRequest registerRequest);
        Task<JwtAuthResponse> LoginAsync(LoginRequest loginRequest);
    }
}

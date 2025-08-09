using BackendDotNet.DTOs.Request;
using BackendDotNet.Models;

namespace BackendDotNet.Services
{
    public interface IHospitalService
    {
        Task<BloodRequest> CreateBloodRequestAsync(BloodRequestCreateRequest requestDto);
        Task<List<BloodRequest>> GetMyBloodRequestsAsync();
    }
}

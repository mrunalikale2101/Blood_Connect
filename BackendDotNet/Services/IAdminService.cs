using BackendDotNet.DTOs.Request;
using BackendDotNet.DTOs.Response;
using BackendDotNet.Models;

namespace BackendDotNet.Services
{
    public interface IAdminService
    {
        // Inventory
        Task<List<BloodInventory>> GetAllBloodInventoryAsync();
        Task<BloodInventory> UpdateBloodInventoryAsync(BloodInventoryUpdateRequest request);
        
        // Blood Requests
        Task<List<BloodRequestDto>> GetAllPendingBloodRequestsAsync();
        Task<BloodRequestDto> UpdateBloodRequestStatusAsync(long requestId, string newStatus);
        
        // Donor Management
        Task<List<DonorInfoResponseDto>> GetAllDonorsAsync();
        Task<DonorInfoResponseDto> UpdateDonorEligibilityAsync(long donorUserId, bool isEligible);
        
        // Appointment Management
        Task<List<DonationAppointmentDto>> GetAllScheduledAppointmentsAsync();
        Task<DonationRecordDto> CompleteDonationAppointmentAsync(long appointmentId);
        
        // Dashboard
        Task<DashboardStatsDto> GetDashboardStatsAsync();
    }
}

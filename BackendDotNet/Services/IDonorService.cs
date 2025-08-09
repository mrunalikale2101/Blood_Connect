using BackendDotNet.DTOs.Request;
using BackendDotNet.DTOs.Response;
using BackendDotNet.Models;

namespace BackendDotNet.Services
{
    public interface IDonorService
    {
        Task<DonorProfile> GetMyProfileAsync();
        Task<List<DonationRecordDto>> GetMyDonationHistoryAsync();
        Task<DonationAppointment> BookAppointmentAsync(AppointmentCreateRequest request);
        Task<DonationAppointment> CancelAppointmentAsync(long appointmentId);
        Task<List<DonationAppointment>> GetMyScheduledAppointmentsAsync();
    }
}

using BackendDotNet.Models;

namespace BackendDotNet.Repositories
{
    public interface IDonationAppointmentRepository
    {
        Task<DonationAppointment> CreateAsync(DonationAppointment appointment);
        Task<DonationAppointment?> GetByIdAsync(long appointmentId);
        Task<List<DonationAppointment>> GetByDonorIdAsync(long donorId);
        Task<DonationAppointment> UpdateAsync(DonationAppointment appointment);
        Task<List<DonationAppointment>> GetAllAsync();
        Task<List<DonationAppointment>> GetByStatusAsync(string status);
    }
}

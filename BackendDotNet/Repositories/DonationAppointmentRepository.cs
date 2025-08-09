using BackendDotNet.Data;
using BackendDotNet.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendDotNet.Repositories
{
    public class DonationAppointmentRepository : IDonationAppointmentRepository
    {
        private readonly ApplicationDbContext _context;

        public DonationAppointmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DonationAppointment> CreateAsync(DonationAppointment appointment)
        {
            _context.DonationAppointments.Add(appointment);
            await _context.SaveChangesAsync();
            return appointment;
        }

        public async Task<DonationAppointment?> GetByIdAsync(long appointmentId)
        {
            return await _context.DonationAppointments
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);
        }

        public async Task<List<DonationAppointment>> GetByDonorIdAsync(long donorId)
        {
            return await _context.DonationAppointments
                .AsNoTracking()
                .Where(a => a.DonorUserId == donorId)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();
        }

        public async Task<DonationAppointment> UpdateAsync(DonationAppointment appointment)
        {
            _context.DonationAppointments.Update(appointment);
            await _context.SaveChangesAsync();
            return appointment;
        }

        public async Task<List<DonationAppointment>> GetAllAsync()
        {
            return await _context.DonationAppointments
                .AsNoTracking()
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();
        }

        public async Task<List<DonationAppointment>> GetByStatusAsync(string status)
        {
            return await _context.DonationAppointments
                .AsNoTracking()
                .Where(a => a.Status == status)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();
        }
    }
}

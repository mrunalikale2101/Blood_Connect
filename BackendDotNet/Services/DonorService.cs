using BackendDotNet.DTOs.Request;
using BackendDotNet.DTOs.Response;
using BackendDotNet.Models;
using BackendDotNet.Repositories;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BackendDotNet.Services
{
    public class DonorService : IDonorService
    {
        private readonly IDonorProfileRepository _donorProfileRepository;
        private readonly IDonationAppointmentRepository _appointmentRepository;
        private readonly IDonationRecordRepository _donationRecordRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DonorService(
            IDonorProfileRepository donorProfileRepository,
            IDonationAppointmentRepository appointmentRepository,
            IDonationRecordRepository donationRecordRepository,
            IUserRepository userRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _donorProfileRepository = donorProfileRepository;
            _appointmentRepository = appointmentRepository;
            _donationRecordRepository = donationRecordRepository;
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        private long GetCurrentUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !long.TryParse(userIdClaim.Value, out var userId))
            {
                throw new UnauthorizedAccessException("User not authenticated");
            }
            return userId;
        }

        public async Task<DonorProfile> GetMyProfileAsync()
        {
            var userId = GetCurrentUserId();
            var profile = await _donorProfileRepository.GetByUserIdAsync(userId);
            if (profile == null)
            {
                throw new InvalidOperationException("Donor profile not found");
            }
            return profile;
        }

        public async Task<List<DonationRecordDto>> GetMyDonationHistoryAsync()
        {
            var userId = GetCurrentUserId();
            var records = await _donationRecordRepository.GetByDonorIdAsync(userId);
            var user = await _userRepository.GetByIdAsync(userId);
            
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }

            return records.Select(r => new DonationRecordDto
            {
                RecordId = r.RecordId,
                Donor = new DonorSimpleDto
                {
                    UserId = user.UserId,
                    Email = user.Email,
                    FullName = user.DonorProfile?.FullName ?? "Unknown"
                },
                DonationDate = r.DonationDate,
                UnitsDonated = r.UnitsDonated,
                CreatedAt = r.CreatedAt
            }).ToList();
        }

        public async Task<DonationAppointment> BookAppointmentAsync(AppointmentCreateRequest request)
        {
            var userId = GetCurrentUserId();
            
            // Check if donor profile exists
            var profile = await _donorProfileRepository.GetByUserIdAsync(userId);
            if (profile == null)
            {
                throw new InvalidOperationException("Donor profile not found");
            }

            // Check if donor is eligible
            if (!profile.IsEligible)
            {
                throw new InvalidOperationException("Donor is not eligible for donation");
            }

            // Check if appointment date is in the future
            if (request.AppointmentDate <= DateTime.Today)
            {
                throw new InvalidOperationException("Appointment date must be in the future");
            }

            var appointment = new DonationAppointment
            {
                DonorUserId = userId,
                AppointmentDate = request.AppointmentDate,
                Status = "SCHEDULED",
                CreatedAt = DateTime.UtcNow
            };

            return await _appointmentRepository.CreateAsync(appointment);
        }

        public async Task<DonationAppointment> CancelAppointmentAsync(long appointmentId)
        {
            var userId = GetCurrentUserId();
            var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);
            
            if (appointment == null)
            {
                throw new InvalidOperationException("Appointment not found");
            }

            if (appointment.DonorUserId != userId)
            {
                throw new UnauthorizedAccessException("You can only cancel your own appointments");
            }

            if (appointment.Status != "SCHEDULED")
            {
                throw new InvalidOperationException("Only scheduled appointments can be cancelled");
            }

            appointment.Status = "CANCELLED";
            return await _appointmentRepository.UpdateAsync(appointment);
        }

        public async Task<List<DonationAppointment>> GetMyScheduledAppointmentsAsync()
        {
            var userId = GetCurrentUserId();
            var appointments = await _appointmentRepository.GetByDonorIdAsync(userId);
            return appointments.Where(a => a.Status == "SCHEDULED").ToList();
        }
    }
}

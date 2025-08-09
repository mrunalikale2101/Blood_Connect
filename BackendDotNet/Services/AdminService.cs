using AutoMapper;
using BackendDotNet.DTOs.Request;
using BackendDotNet.DTOs.Response;
using BackendDotNet.Models;
using BackendDotNet.Repositories;

namespace BackendDotNet.Services
{
    public class AdminService : IAdminService
    {
        private readonly IBloodInventoryRepository _bloodInventoryRepository;
        private readonly IBloodRequestRepository _bloodRequestRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDonorProfileRepository _donorProfileRepository;
        private readonly IHospitalProfileRepository _hospitalProfileRepository;
        private readonly IDonationAppointmentRepository _donationAppointmentRepository;
        private readonly IDonationRecordRepository _donationRecordRepository;
        private readonly IMapper _mapper;

        public AdminService(
            IBloodInventoryRepository bloodInventoryRepository,
            IBloodRequestRepository bloodRequestRepository,
            IUserRepository userRepository,
            IDonorProfileRepository donorProfileRepository,
            IHospitalProfileRepository hospitalProfileRepository,
            IDonationAppointmentRepository donationAppointmentRepository,
            IDonationRecordRepository donationRecordRepository,
            IMapper mapper)
        {
            _bloodInventoryRepository = bloodInventoryRepository;
            _bloodRequestRepository = bloodRequestRepository;
            _userRepository = userRepository;
            _donorProfileRepository = donorProfileRepository;
            _hospitalProfileRepository = hospitalProfileRepository;
            _donationAppointmentRepository = donationAppointmentRepository;
            _donationRecordRepository = donationRecordRepository;
            _mapper = mapper;
        }

        public async Task<List<BloodInventory>> GetAllBloodInventoryAsync()
        {
            return await _bloodInventoryRepository.GetAllAsync();
        }

        public async Task<BloodInventory> UpdateBloodInventoryAsync(BloodInventoryUpdateRequest request)
        {
            var inventory = await _bloodInventoryRepository.GetByBloodGroupAsync(request.BloodGroup);
            
            if (inventory == null)
            {
                inventory = new BloodInventory(request.BloodGroup)
                {
                    Units = request.Units
                };
                return await _bloodInventoryRepository.AddAsync(inventory);
            }
            
            inventory.Units = request.Units;
            return await _bloodInventoryRepository.UpdateAsync(inventory);
        }

        public async Task<List<BloodRequestDto>> GetAllPendingBloodRequestsAsync()
        {
            var requests = await _bloodRequestRepository.GetByStatusAsync("PENDING");
            return requests.Select(ConvertRequestToDto).ToList();
        }

        public async Task<BloodRequestDto> UpdateBloodRequestStatusAsync(long requestId, string newStatus)
        {
            var bloodRequest = await _bloodRequestRepository.GetByIdAsync(requestId);
            if (bloodRequest == null)
            {
                throw new KeyNotFoundException($"BloodRequest with id {requestId} not found");
            }

            if (!"PENDING".Equals(bloodRequest.Status, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("Request is not in a PENDING state and cannot be updated.");
            }

            if ("APPROVED".Equals(newStatus, StringComparison.OrdinalIgnoreCase))
            {
                var inventory = await _bloodInventoryRepository.GetByBloodGroupAsync(bloodRequest.BloodGroup);
                if (inventory == null)
                {
                    throw new KeyNotFoundException($"BloodInventory for blood group {bloodRequest.BloodGroup} not found");
                }

                int requestedUnits = bloodRequest.UnitsRequested;
                if (inventory.Units < requestedUnits)
                {
                    throw new InvalidOperationException("Not enough units in inventory to approve this request.");
                }

                inventory.Units -= requestedUnits;
                await _bloodInventoryRepository.UpdateAsync(inventory);
                bloodRequest.IsFulfilled = true;
            }

            bloodRequest.Status = newStatus.ToUpper();
            var updatedRequest = await _bloodRequestRepository.UpdateAsync(bloodRequest);

            // TODO: Implement email notification service
            // await SendNotificationEmail(updatedRequest, newStatus);

            return ConvertRequestToDto(updatedRequest);
        }

        public async Task<List<DonorInfoResponseDto>> GetAllDonorsAsync()
        {
            var donors = await _donorProfileRepository.GetAllWithUserAsync();
            return donors.Select(ConvertDonorToInfoDto).ToList();
        }

        public async Task<DonorInfoResponseDto> UpdateDonorEligibilityAsync(long donorUserId, bool isEligible)
        {
            var donorProfile = await _donorProfileRepository.GetByUserIdAsync(donorUserId);
            if (donorProfile == null)
            {
                throw new KeyNotFoundException($"Donor profile for user id {donorUserId} not found");
            }

            donorProfile.IsEligible = isEligible;
            var updatedDonor = await _donorProfileRepository.UpdateAsync(donorProfile);
            
            return ConvertDonorToInfoDto(updatedDonor);
        }

        public async Task<List<DonationAppointmentDto>> GetAllScheduledAppointmentsAsync()
        {
            var appointments = await _donationAppointmentRepository.GetByStatusAsync("SCHEDULED");
            return appointments.Select(ConvertAppointmentToDto).ToList();
        }

        public async Task<DonationRecordDto> CompleteDonationAppointmentAsync(long appointmentId)
        {
            var appointment = await _donationAppointmentRepository.GetByIdAsync(appointmentId);
            if (appointment == null)
            {
                throw new KeyNotFoundException($"Appointment with id {appointmentId} not found");
            }

            if (appointment.Status != "SCHEDULED")
            {
                throw new InvalidOperationException("Only scheduled appointments can be completed");
            }

            // Update appointment status
            appointment.Status = "COMPLETED";
            await _donationAppointmentRepository.UpdateAsync(appointment);

            // Create donation record
            var donationRecord = new DonationRecord
            {
                DonorUserId = appointment.DonorUserId,
                DonationDate = appointment.AppointmentDate,
                UnitsDonated = 1, // Default to 1 unit
                CreatedAt = DateTime.UtcNow
            };

            var createdRecord = await _donationRecordRepository.CreateAsync(donationRecord);

            // Update blood inventory
            var donorProfile = await _donorProfileRepository.GetByUserIdAsync(appointment.DonorUserId);
            if (donorProfile != null)
            {
                var inventory = await _bloodInventoryRepository.GetByBloodGroupAsync(donorProfile.BloodGroup);
                if (inventory != null)
                {
                    inventory.Units += 1;
                    await _bloodInventoryRepository.UpdateAsync(inventory);
                }
            }

            return ConvertDonationRecordToDto(createdRecord);
        }

        public async Task<DashboardStatsDto> GetDashboardStatsAsync()
        {
            var totalDonors = await _donorProfileRepository.CountAsync();
            var totalHospitals = await _hospitalProfileRepository.CountAsync();
            var pendingRequests = await _bloodRequestRepository.CountByStatusAsync("PENDING");
            var totalBloodUnits = await _bloodInventoryRepository.GetTotalUnitsAsync();

            return new DashboardStatsDto
            {
                TotalDonors = (int)totalDonors,
                TotalHospitals = (int)totalHospitals,
                PendingRequests = (int)pendingRequests,
                TotalBloodUnits = totalBloodUnits
            };
        }

        private BloodRequestDto ConvertRequestToDto(BloodRequest request)
        {
            return new BloodRequestDto
            {
                RequestId = request.RequestId,
                BloodGroup = request.BloodGroup,
                UnitsRequested = request.UnitsRequested,
                Urgency = request.Urgency,
                UrgencyLevel = request.Urgency,
                Status = request.Status,
                RequestDate = request.CreatedAt,
                RequiredDate = request.CreatedAt,
                CreatedAt = request.CreatedAt,
                IsFulfilled = request.IsFulfilled,
                HospitalName = request.Hospital?.HospitalProfile?.HospitalName ?? "Unknown",
                ContactPerson = request.Hospital?.HospitalProfile?.ContactPerson ?? "Unknown",
                ContactNumber = request.Hospital?.HospitalProfile?.ContactNumber ?? "Unknown"
            };
        }

        private DonorInfoResponseDto ConvertDonorToInfoDto(DonorProfile donor)
        {
            return new DonorInfoResponseDto
            {
                UserId = donor.UserId,
                Email = donor.User?.Email ?? "Unknown",
                FullName = donor.FullName,
                BloodGroup = donor.BloodGroup,
                Age = donor.Age,
                Gender = donor.Gender,
                ContactNumber = donor.ContactNumber,
                Address = donor.Address,
                IsEligible = donor.IsEligible,
                LastDonationDate = donor.LastDonationDate,
                CreatedAt = donor.CreatedAt
            };
        }

        private DonationAppointmentDto ConvertAppointmentToDto(DonationAppointment appointment)
        {
            return new DonationAppointmentDto
            {
                AppointmentId = appointment.AppointmentId,
                Donor = new DonorSimpleDto
                {
                    UserId = appointment.DonorUserId,
                    Email = "Unknown", // Would need to fetch from User repository
                    FullName = "Unknown" // Would need to fetch from DonorProfile repository
                },
                AppointmentDate = appointment.AppointmentDate,
                Status = appointment.Status,
                CreatedAt = appointment.CreatedAt
            };
        }

        private DonationRecordDto ConvertDonationRecordToDto(DonationRecord record)
        {
            return new DonationRecordDto
            {
                RecordId = record.RecordId,
                Donor = new DonorSimpleDto
                {
                    UserId = record.DonorUserId,
                    Email = "Unknown", // Would need to fetch from User repository
                    FullName = "Unknown" // Would need to fetch from DonorProfile repository
                },
                DonationDate = record.DonationDate,
                UnitsDonated = record.UnitsDonated,
                CreatedAt = record.CreatedAt
            };
        }
    }
}

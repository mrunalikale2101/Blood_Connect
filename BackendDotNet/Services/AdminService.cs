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
        private readonly IMapper _mapper;

        public AdminService(
            IBloodInventoryRepository bloodInventoryRepository,
            IBloodRequestRepository bloodRequestRepository,
            IUserRepository userRepository,
            IDonorProfileRepository donorProfileRepository,
            IHospitalProfileRepository hospitalProfileRepository,
            IMapper mapper)
        {
            _bloodInventoryRepository = bloodInventoryRepository;
            _bloodRequestRepository = bloodRequestRepository;
            _userRepository = userRepository;
            _donorProfileRepository = donorProfileRepository;
            _hospitalProfileRepository = hospitalProfileRepository;
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
            // TODO: Implement when DonationAppointmentRepository is available
            // var appointments = await _donationAppointmentRepository.GetByStatusAsync("SCHEDULED");
            // return appointments.Select(ConvertAppointmentToDto).ToList();
            return new List<DonationAppointmentDto>();
        }

        public async Task<DonationRecordDto> CompleteDonationAppointmentAsync(long appointmentId)
        {
            // TODO: Implement when DonationAppointmentRepository and DonationRecordRepository are available
            throw new NotImplementedException("Appointment completion will be implemented with donation repositories");
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
                UrgencyLevel = request.UrgencyLevel,
                Status = request.Status,
                RequestDate = request.RequestDate,
                RequiredDate = request.RequiredDate,
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
                LastDonationDate = donor.LastDonationDate
            };
        }
    }
}

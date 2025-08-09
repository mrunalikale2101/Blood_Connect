using BackendDotNet.DTOs.Request;
using BackendDotNet.Models;
using BackendDotNet.Repositories;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BackendDotNet.Services
{
    public class HospitalService : IHospitalService
    {
        private readonly IBloodRequestRepository _bloodRequestRepository;
        private readonly IHospitalProfileRepository _hospitalProfileRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HospitalService(
            IBloodRequestRepository bloodRequestRepository,
            IHospitalProfileRepository hospitalProfileRepository,
            IUserRepository userRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _bloodRequestRepository = bloodRequestRepository;
            _hospitalProfileRepository = hospitalProfileRepository;
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

        public async Task<BloodRequest> CreateBloodRequestAsync(BloodRequestCreateRequest requestDto)
        {
            var userId = GetCurrentUserId();
            
            // Check if hospital profile exists and is verified
            var profile = await _hospitalProfileRepository.GetByUserIdAsync(userId);
            if (profile == null)
            {
                throw new InvalidOperationException("Hospital profile not found");
            }

            if (!profile.IsVerified)
            {
                throw new InvalidOperationException("Hospital profile must be verified to create blood requests");
            }

            // Validate urgency level
            var validUrgencyLevels = new[] { "LOW", "MEDIUM", "HIGH", "CRITICAL" };
            if (!validUrgencyLevels.Contains(requestDto.Urgency.ToUpper()))
            {
                throw new InvalidOperationException("Invalid urgency level. Must be LOW, MEDIUM, HIGH, or CRITICAL");
            }

            // Validate blood group
            var validBloodGroups = new[] { "A+", "A-", "B+", "B-", "AB+", "AB-", "O+", "O-" };
            if (!validBloodGroups.Contains(requestDto.BloodGroup.ToUpper()))
            {
                throw new InvalidOperationException("Invalid blood group");
            }

            var bloodRequest = new BloodRequest
            {
                HospitalUserId = userId,
                BloodGroup = requestDto.BloodGroup.ToUpper(),
                UnitsRequested = requestDto.Units,
                Urgency = requestDto.Urgency.ToUpper(),
                Status = "PENDING",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsFulfilled = false
            };

            return await _bloodRequestRepository.CreateAsync(bloodRequest);
        }

        public async Task<List<BloodRequest>> GetMyBloodRequestsAsync()
        {
            var userId = GetCurrentUserId();
            return await _bloodRequestRepository.GetByHospitalUserIdAsync(userId);
        }
    }
}

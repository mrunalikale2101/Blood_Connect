using BackendDotNet.DTOs.Request;
using BackendDotNet.DTOs.Response;
using BackendDotNet.Models;
using BackendDotNet.Repositories;
using Microsoft.AspNetCore.Identity;

namespace BackendDotNet.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IDonorProfileRepository _donorProfileRepository;
        private readonly IHospitalProfileRepository _hospitalProfileRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IJwtTokenProvider _jwtTokenProvider;

        public AuthService(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IDonorProfileRepository donorProfileRepository,
            IHospitalProfileRepository hospitalProfileRepository,
            IPasswordHasher<User> passwordHasher,
            IJwtTokenProvider jwtTokenProvider)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _donorProfileRepository = donorProfileRepository;
            _hospitalProfileRepository = hospitalProfileRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenProvider = jwtTokenProvider;
        }

        public async Task<string> RegisterDonorAsync(DonorRegisterRequest registerRequest)
        {
            if (await _userRepository.ExistsByEmailAsync(registerRequest.Email))
            {
                throw new InvalidOperationException("Error: Email is already in use!");
            }

            var userRole = await _roleRepository.GetByRoleNameAsync("ROLE_DONOR")
                ?? throw new InvalidOperationException("Error: Role is not found.");

            var user = new User
            {
                Email = registerRequest.Email,
                RoleId = userRole.RoleId,
                IsActive = true
            };

            user.Password = _passwordHasher.HashPassword(user, registerRequest.Password);
            var savedUser = await _userRepository.AddAsync(user);

            var donorProfile = new DonorProfile
            {
                UserId = savedUser.UserId,
                FullName = registerRequest.FullName,
                BloodGroup = registerRequest.BloodGroup,
                ContactNumber = registerRequest.ContactNumber
            };

            await _donorProfileRepository.AddAsync(donorProfile);

            return "Donor registered successfully!";
        }

        public async Task<string> RegisterHospitalAsync(HospitalRegisterRequest registerRequest)
        {
            if (await _userRepository.ExistsByEmailAsync(registerRequest.Email))
            {
                throw new InvalidOperationException("Error: Email is already in use!");
            }

            var userRole = await _roleRepository.GetByRoleNameAsync("ROLE_HOSPITAL")
                ?? throw new InvalidOperationException("Error: Role is not found.");

            var user = new User
            {
                Email = registerRequest.Email,
                RoleId = userRole.RoleId,
                IsActive = true
            };

            user.Password = _passwordHasher.HashPassword(user, registerRequest.Password);
            var savedUser = await _userRepository.AddAsync(user);

            var hospitalProfile = new HospitalProfile
            {
                UserId = savedUser.UserId,
                HospitalName = registerRequest.HospitalName,
                Address = registerRequest.Address,
                LicenseNumber = registerRequest.LicenseNumber,
                ContactPerson = registerRequest.ContactPerson
            };

            await _hospitalProfileRepository.AddAsync(hospitalProfile);

            return "Hospital registered successfully!";
        }

        public async Task<JwtAuthResponse> LoginAsync(LoginRequest loginRequest)
        {
            var user = await _userRepository.GetByEmailAsync(loginRequest.Email)
                ?? throw new UnauthorizedAccessException("Invalid email or password");

            if (!user.IsActive)
            {
                throw new UnauthorizedAccessException("Account is not active");
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, loginRequest.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            var token = _jwtTokenProvider.GenerateToken(user);
            var userDetails = await GetUserSpecificProfileAsync(user);

            return new JwtAuthResponse
            {
                AccessToken = token,
                UserDetails = userDetails
            };
        }

        private async Task<object?> GetUserSpecificProfileAsync(User user)
        {
            var roleName = user.Role.RoleName;
            
            if (roleName == "ROLE_DONOR")
            {
                return await _donorProfileRepository.GetByUserIdAsync(user.UserId)
                    ?? throw new InvalidOperationException($"DonorProfile not found for userId: {user.UserId}");
            }
            else if (roleName == "ROLE_HOSPITAL")
            {
                return await _hospitalProfileRepository.GetByUserIdAsync(user.UserId)
                    ?? throw new InvalidOperationException($"HospitalProfile not found for userId: {user.UserId}");
            }

            // For Admin or other roles, return the basic user object
            return user;
        }
    }
}

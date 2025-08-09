using BackendDotNet.DTOs.Request;
using BackendDotNet.DTOs.Response;
using BackendDotNet.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackendDotNet.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register/donor")]
        public async Task<ActionResult<string>> RegisterDonor([FromBody] DonorRegisterRequest registerRequest)
        {
            try
            {
                var response = await _authService.RegisterDonorAsync(registerRequest);
                return StatusCode(201, response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("register/hospital")]
        public async Task<ActionResult<string>> RegisterHospital([FromBody] HospitalRegisterRequest registerRequest)
        {
            try
            {
                var response = await _authService.RegisterHospitalAsync(registerRequest);
                return StatusCode(201, response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<JwtAuthResponse>> Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                var response = await _authService.LoginAsync(loginRequest);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}

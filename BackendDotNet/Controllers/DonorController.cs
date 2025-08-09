using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BackendDotNet.DTOs.Request;
using BackendDotNet.DTOs.Response;
using BackendDotNet.Models;
using BackendDotNet.Services;

namespace BackendDotNet.Controllers
{
    [ApiController]
    [Route("api/donor")]
    [Authorize(Roles = "ROLE_DONOR")]
    public class DonorController : ControllerBase
    {
        private readonly IDonorService _donorService;

        public DonorController(IDonorService donorService)
        {
            _donorService = donorService;
        }

        [HttpGet("profile")]
        public async Task<ActionResult<DonorProfile>> GetMyProfile()
        {
            try
            {
                var profile = await _donorService.GetMyProfileAsync();
                return Ok(profile);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving profile", error = ex.Message });
            }
        }

        [HttpPost("appointment")]
        public async Task<ActionResult<DonationAppointment>> BookAppointment([FromBody] AppointmentCreateRequest request)
        {
            try
            {
                var appointment = await _donorService.BookAppointmentAsync(request);
                return StatusCode(201, appointment);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error booking appointment", error = ex.Message });
            }
        }

        [HttpGet("donations/history")]
        public async Task<ActionResult<List<DonationRecordDto>>> GetMyDonationHistory()
        {
            try
            {
                var history = await _donorService.GetMyDonationHistoryAsync();
                return Ok(history);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving donation history", error = ex.Message });
            }
        }

        [HttpPatch("appointment/{appointmentId}/cancel")]
        public async Task<ActionResult<DonationAppointment>> CancelAppointment(long appointmentId)
        {
            try
            {
                var cancelledAppointment = await _donorService.CancelAppointmentAsync(appointmentId);
                return Ok(cancelledAppointment);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error cancelling appointment", error = ex.Message });
            }
        }

        [HttpGet("appointments/scheduled")]
        public async Task<ActionResult<List<DonationAppointment>>> GetMyScheduledAppointments()
        {
            try
            {
                var appointments = await _donorService.GetMyScheduledAppointmentsAsync();
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving scheduled appointments", error = ex.Message });
            }
        }
    }
}

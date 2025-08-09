using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BackendDotNet.DTOs.Request;
using BackendDotNet.DTOs.Response;
using BackendDotNet.Models;
using BackendDotNet.Services;

namespace BackendDotNet.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = "ROLE_ADMIN")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        // --- Inventory Endpoints ---
        [HttpGet("inventory")]
        public async Task<ActionResult<List<BloodInventory>>> GetAllInventory()
        {
            try
            {
                var inventory = await _adminService.GetAllBloodInventoryAsync();
                return Ok(inventory);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving inventory", error = ex.Message });
            }
        }

        [HttpPut("inventory")]
        public async Task<ActionResult<BloodInventory>> UpdateInventory([FromBody] BloodInventoryUpdateRequest request)
        {
            try
            {
                var updatedInventory = await _adminService.UpdateBloodInventoryAsync(request);
                return Ok(updatedInventory);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating inventory", error = ex.Message });
            }
        }

        // --- Blood Request Endpoints ---
        [HttpGet("requests/pending")]
        public async Task<ActionResult<List<BloodRequestDto>>> GetPendingRequests()
        {
            try
            {
                var requests = await _adminService.GetAllPendingBloodRequestsAsync();
                return Ok(requests);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving pending requests", error = ex.Message });
            }
        }

        [HttpPatch("requests/{requestId}")]
        public async Task<ActionResult<BloodRequestDto>> UpdateRequestStatus(
            long requestId,
            [FromBody] BloodRequestUpdateStatusRequest request)
        {
            try
            {
                var updatedRequest = await _adminService.UpdateBloodRequestStatusAsync(requestId, request.NewStatus);
                return Ok(updatedRequest);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating request status", error = ex.Message });
            }
        }

        // --- Donor Management Endpoints ---
        [HttpGet("donors")]
        public async Task<ActionResult<List<DonorInfoResponseDto>>> GetAllDonors()
        {
            try
            {
                var donors = await _adminService.GetAllDonorsAsync();
                return Ok(donors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving donors", error = ex.Message });
            }
        }

        [HttpPatch("donors/{userId}/eligibility")]
        public async Task<ActionResult<DonorInfoResponseDto>> UpdateDonorEligibility(
            long userId,
            [FromBody] DonorEligibilityUpdateRequest request)
        {
            try
            {
                var updatedDonor = await _adminService.UpdateDonorEligibilityAsync(userId, request.IsEligible);
                return Ok(updatedDonor);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating donor eligibility", error = ex.Message });
            }
        }

        // --- Appointment Management Endpoints ---
        [HttpGet("appointments/scheduled")]
        public async Task<ActionResult<List<DonationAppointmentDto>>> GetScheduledAppointments()
        {
            try
            {
                var appointments = await _adminService.GetAllScheduledAppointmentsAsync();
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving scheduled appointments", error = ex.Message });
            }
        }

        [HttpPost("appointments/{appointmentId}/complete")]
        public async Task<ActionResult<DonationRecordDto>> CompleteAppointment(long appointmentId)
        {
            try
            {
                var donationRecord = await _adminService.CompleteDonationAppointmentAsync(appointmentId);
                return CreatedAtAction(nameof(CompleteAppointment), new { appointmentId }, donationRecord);
            }
            catch (NotImplementedException ex)
            {
                return StatusCode(501, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error completing appointment", error = ex.Message });
            }
        }

        // --- Dashboard Stats Endpoint ---
        [HttpGet("dashboard/stats")]
        public async Task<ActionResult<DashboardStatsDto>> GetDashboardStats()
        {
            try
            {
                var stats = await _adminService.GetDashboardStatsAsync();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving dashboard stats", error = ex.Message });
            }
        }
    }
}

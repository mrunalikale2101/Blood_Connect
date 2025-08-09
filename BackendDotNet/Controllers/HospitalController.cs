using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BackendDotNet.DTOs.Request;
using BackendDotNet.Models;
using BackendDotNet.Services;

namespace BackendDotNet.Controllers
{
    [ApiController]
    [Route("api/hospital")]
    [Authorize(Roles = "ROLE_HOSPITAL")]
    public class HospitalController : ControllerBase
    {
        private readonly IHospitalService _hospitalService;

        public HospitalController(IHospitalService hospitalService)
        {
            _hospitalService = hospitalService;
        }

        [HttpPost("request")]
        public async Task<ActionResult<BloodRequest>> CreateBloodRequest([FromBody] BloodRequestCreateRequest request)
        {
            try
            {
                var newRequest = await _hospitalService.CreateBloodRequestAsync(request);
                return StatusCode(201, newRequest);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating blood request", error = ex.Message });
            }
        }

        [HttpGet("requests")]
        public async Task<ActionResult<List<BloodRequest>>> GetMyBloodRequests()
        {
            try
            {
                var requests = await _hospitalService.GetMyBloodRequestsAsync();
                return Ok(requests);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving blood requests", error = ex.Message });
            }
        }
    }
}

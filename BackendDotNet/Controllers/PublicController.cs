using BackendDotNet.DTOs.Request;
using Microsoft.AspNetCore.Mvc;

namespace BackendDotNet.Controllers
{
    [ApiController]
    [Route("api/public")]
    public class PublicController : ControllerBase
    {
        [HttpPost("contact")]
        public ActionResult<string> SubmitContactMessage([FromBody] ContactMessageRequestDto requestDto)
        {
            try
            {
                // TODO: Implement contact message service
                return StatusCode(201, "Your message has been received. Thank you!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}

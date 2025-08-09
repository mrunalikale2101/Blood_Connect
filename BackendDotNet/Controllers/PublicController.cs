using BackendDotNet.DTOs.Request;
using BackendDotNet.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackendDotNet.Controllers
{
    [ApiController]
    [Route("api/public")]
    public class PublicController : ControllerBase
    {
        private readonly IPublicService _publicService;

        public PublicController(IPublicService publicService)
        {
            _publicService = publicService;
        }

        [HttpPost("contact")]
        public async Task<ActionResult<string>> SubmitContactMessage([FromBody] ContactMessageRequestDto requestDto)
        {
            try
            {
                var result = await _publicService.SaveContactMessageAsync(requestDto);
                return StatusCode(201, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}

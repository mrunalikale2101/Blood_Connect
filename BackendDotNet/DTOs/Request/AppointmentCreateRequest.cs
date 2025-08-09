using System.ComponentModel.DataAnnotations;

namespace BackendDotNet.DTOs.Request
{
    public class AppointmentCreateRequest
    {
        [Required]
        public DateTime AppointmentDate { get; set; }
    }
}

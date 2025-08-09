using System.ComponentModel.DataAnnotations;

namespace BackendDotNet.DTOs.Request
{
    public class BloodRequestCreateRequest
    {
        [Required]
        public string BloodGroup { get; set; } = string.Empty;
        
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Units must be greater than 0")]
        public int Units { get; set; }
        
        [Required]
        public string Urgency { get; set; } = string.Empty;
    }
}

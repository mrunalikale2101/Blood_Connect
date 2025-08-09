namespace BackendDotNet.DTOs.Request
{
    public class DonorRegisterRequest
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string BloodGroup { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
    }
}

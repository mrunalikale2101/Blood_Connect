namespace BackendDotNet.DTOs.Request
{
    public class HospitalRegisterRequest
    {
        public string HospitalName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public string ContactPerson { get; set; } = string.Empty;
    }
}

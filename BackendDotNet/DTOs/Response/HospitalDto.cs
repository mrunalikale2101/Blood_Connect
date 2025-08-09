namespace BackendDotNet.DTOs.Response
{
    public class HospitalDto
    {
        public long UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string HospitalName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string ContactPerson { get; set; } = string.Empty;
        public bool IsVerified { get; set; }
    }
}

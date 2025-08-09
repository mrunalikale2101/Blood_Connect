namespace BackendDotNet.DTOs.Response
{
    public class DonorInfoResponseDto
    {
        public long UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string BloodGroup { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
        public DateTime? LastDonationDate { get; set; }
        public bool IsEligible { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

namespace BackendDotNet.DTOs.Response
{
    public class DonorSimpleDto
    {
        public long UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
    }
}

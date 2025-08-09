namespace BackendDotNet.DTOs.Response
{
    public class JwtAuthResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public string TokenType { get; set; } = "Bearer";
        public UserDetailsDto UserDetails { get; set; } = new();
    }

    public class UserDetailsDto
    {
        public long UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

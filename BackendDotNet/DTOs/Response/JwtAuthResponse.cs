namespace BackendDotNet.DTOs.Response
{
    public class JwtAuthResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public string TokenType { get; set; } = "Bearer";
        public object? UserDetails { get; set; }
    }
}

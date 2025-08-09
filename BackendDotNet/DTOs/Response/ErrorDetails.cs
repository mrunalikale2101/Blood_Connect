namespace BackendDotNet.DTOs.Response
{
    public class ErrorDetails
    {
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string Message { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
    }
}

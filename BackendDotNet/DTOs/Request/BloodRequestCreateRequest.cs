namespace BackendDotNet.DTOs.Request
{
    public class BloodRequestCreateRequest
    {
        public string BloodGroup { get; set; } = string.Empty;
        public int Units { get; set; }
        public string Urgency { get; set; } = string.Empty;
    }
}

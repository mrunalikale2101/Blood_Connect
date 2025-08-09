namespace BackendDotNet.DTOs.Request
{
    public class BloodInventoryUpdateRequest
    {
        public string BloodGroup { get; set; } = string.Empty;
        public int Units { get; set; }
    }
}

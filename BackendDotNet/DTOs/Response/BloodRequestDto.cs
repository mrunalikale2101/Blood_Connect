namespace BackendDotNet.DTOs.Response
{
    public class BloodRequestDto
    {
        public long RequestId { get; set; }
        public HospitalDto Hospital { get; set; } = new();
        public string BloodGroup { get; set; } = string.Empty;
        public int UnitsRequested { get; set; }
        public string Urgency { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsFulfilled { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime RequiredDate { get; set; }
        public string HospitalName { get; set; } = string.Empty;
        public string ContactPerson { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
        public string UrgencyLevel { get; set; } = string.Empty;
    }
}

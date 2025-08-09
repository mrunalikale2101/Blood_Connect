namespace BackendDotNet.DTOs.Response
{
    public class DonationAppointmentDto
    {
        public long AppointmentId { get; set; }
        public DonorSimpleDto Donor { get; set; } = new();
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}

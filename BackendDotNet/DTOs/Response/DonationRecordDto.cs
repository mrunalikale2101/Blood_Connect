namespace BackendDotNet.DTOs.Response
{
    public class DonationRecordDto
    {
        public long RecordId { get; set; }
        public DonorSimpleDto Donor { get; set; } = new();
        public DateTime DonationDate { get; set; }
        public int UnitsDonated { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

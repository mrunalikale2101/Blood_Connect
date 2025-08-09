using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendDotNet.Models
{
    [Table("donation_records")]
    public class DonationRecord
    {
        [Key]
        [Column("record_id")]
        public long RecordId { get; set; }

        [Required]
        [Column("donor_user_id")]
        public long DonorUserId { get; set; }

        [Required]
        [Column("donation_date")]
        public DateTime DonationDate { get; set; }

        [Required]
        [Column("units_donated")]
        public int UnitsDonated { get; set; } = 1;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        [ForeignKey("DonorUserId")]
        public virtual User Donor { get; set; } = null!;
    }
}

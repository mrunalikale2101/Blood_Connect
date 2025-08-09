using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendDotNet.Models
{
    [Table("donation_appointments")]
    public class DonationAppointment
    {
        [Key]
        [Column("appointment_id")]
        public long AppointmentId { get; set; }

        [Required]
        [Column("donor_user_id")]
        public long DonorUserId { get; set; }

        [Required]
        [Column("appointment_date")]
        public DateTime AppointmentDate { get; set; }

        [Required]
        [Column("status")]
        [StringLength(20)]
        public string Status { get; set; } = string.Empty;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        [ForeignKey("DonorUserId")]
        public virtual User Donor { get; set; } = null!;
    }
}

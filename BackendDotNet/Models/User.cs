using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendDotNet.Models
{
    [Table("users")]
    public class User
    {
        [Key]
        [Column("user_id")]
        public long UserId { get; set; }

        [Required]
        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Column("password")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Column("role_id")]
        public int RoleId { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        // Navigation properties
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; } = null!;

        public virtual DonorProfile? DonorProfile { get; set; }
        public virtual HospitalProfile? HospitalProfile { get; set; }
        public virtual ICollection<BloodRequest> BloodRequests { get; set; } = new List<BloodRequest>();
        public virtual ICollection<DonationAppointment> DonationAppointments { get; set; } = new List<DonationAppointment>();
        public virtual ICollection<DonationRecord> DonationRecords { get; set; } = new List<DonationRecord>();
    }
}

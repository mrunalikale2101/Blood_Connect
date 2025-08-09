using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendDotNet.Models
{
    [Table("hospital_profiles")]
    public class HospitalProfile
    {
        [Key]
        [Column("profile_id")]
        public long ProfileId { get; set; }

        [Required]
        [Column("user_id")]
        public long UserId { get; set; }

        [Required]
        [Column("hospital_name")]
        public string HospitalName { get; set; } = string.Empty;

        [Required]
        [Column("address")]
        public string Address { get; set; } = string.Empty;

        [Required]
        [Column("license_number")]
        [StringLength(100)]
        public string LicenseNumber { get; set; } = string.Empty;

        [Required]
        [Column("contact_person")]
        public string ContactPerson { get; set; } = string.Empty;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Column("is_verified")]
        public bool IsVerified { get; set; } = false;

        // Navigation property
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
    }
}

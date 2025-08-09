using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BackendDotNet.Models
{
    [Table("donor_profiles")]
    public class DonorProfile
    {
        [Key]
        [Column("profile_id")]
        public long ProfileId { get; set; }

        [Required]
        [Column("user_id")]
        public long UserId { get; set; }

        [Required]
        [Column("full_name")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [Column("blood_group")]
        [StringLength(5)]
        public string BloodGroup { get; set; } = string.Empty;

        [Required]
        [Column("contact_number")]
        [StringLength(15)]
        public string ContactNumber { get; set; } = string.Empty;

        [Column("last_donation_date")]
        public DateTime? LastDonationDate { get; set; }

        [Column("age")]
        public int Age { get; set; }

        [Column("gender")]
        [StringLength(10)]
        public string Gender { get; set; } = string.Empty;

        [Column("address")]
        public string Address { get; set; } = string.Empty;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Column("is_eligible")]
        public bool IsEligible { get; set; } = true;

        // Navigation property
        [ForeignKey("UserId")]
        [JsonIgnore]
        public virtual User User { get; set; } = null!;
    }
}

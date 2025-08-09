using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendDotNet.Models
{
    [Table("blood_requests")]
    public class BloodRequest
    {
        [Key]
        [Column("request_id")]
        public long RequestId { get; set; }

        [Required]
        [Column("hospital_user_id")]
        public long HospitalUserId { get; set; }

        // Alias for compatibility with repository methods
        public long HospitalId => HospitalUserId;

        [Required]
        [Column("blood_group")]
        [StringLength(5)]
        public string BloodGroup { get; set; } = string.Empty;

        [Required]
        [Column("units_requested")]
        public int UnitsRequested { get; set; }

        [Required]
        [Column("urgency")]
        [StringLength(20)]
        public string UrgencyLevel { get; set; } = string.Empty;

        [Required]
        [Column("status")]
        [StringLength(20)]
        public string Status { get; set; } = string.Empty;

        [Column("request_date")]
        public DateTime RequestDate { get; set; } = DateTime.UtcNow;

        [Column("required_date")]
        public DateTime RequiredDate { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Column("is_fulfilled")]
        public bool IsFulfilled { get; set; } = false;

        // Navigation property
        [ForeignKey("HospitalUserId")]
        public virtual User Hospital { get; set; } = null!;
    }
}

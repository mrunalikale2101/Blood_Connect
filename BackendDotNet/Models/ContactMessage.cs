using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendDotNet.Models
{
    public class ContactMessage
    {
        [Key]
        [Column("message_id")]
        public long MessageId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "TEXT")]
        public string Message { get; set; } = string.Empty;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [Column("is_read")]
        public bool IsRead { get; set; } = false;
    }
}

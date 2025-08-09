using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BackendDotNet.Models
{
    [Table("roles")]
    public class Role
    {
        [Key]
        [Column("role_id")]
        public int RoleId { get; set; }

        [Required]
        [Column("role_name")]
        [StringLength(20)]
        public string RoleName { get; set; } = string.Empty;

        // Navigation property
        [JsonIgnore]
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}

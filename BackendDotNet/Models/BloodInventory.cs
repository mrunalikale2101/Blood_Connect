using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendDotNet.Models
{
    [Table("blood_inventory")]
    public class BloodInventory
    {
        [Key]
        [Column("inventory_id")]
        public int InventoryId { get; set; }

        [Required]
        [Column("blood_group")]
        [StringLength(5)]
        public string BloodGroup { get; set; } = string.Empty;

        [Required]
        [Column("units")]
        public int Units { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Column("status_id")]
        public int StatusId { get; set; } = 1;

        // Constructor for service usage
        public BloodInventory() { }

        public BloodInventory(string bloodGroup)
        {
            BloodGroup = bloodGroup;
        }
    }
}

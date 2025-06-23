
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace DBEntities.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Description { get; set; }

        [MaxLength(50)]
        public string? IconName { get; set; }

        public int SortOrder { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [MaxLength(100)]
        public string CreatedBy { get; set; } = "SYSTEM";
        public virtual ICollection<ShoppingCartItem> CartItems { get; set; } = [];
    }

}

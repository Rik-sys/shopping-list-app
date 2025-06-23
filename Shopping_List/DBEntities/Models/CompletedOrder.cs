
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;


namespace DBEntities.Models
{
    public class CompletedOrder
    {
        public int CompletedOrderId { get; set; }

        [Required, MaxLength(20)]
        public string OrderNumber { get; set; } = string.Empty;

        public Guid SessionId { get; set; }
        public int TotalItems { get; set; }
        public int TotalCategories { get; set; }
        public DateTime CompletedAt { get; set; } = DateTime.UtcNow;

        [MaxLength(1000)]
        public string? Notes { get; set; }

        // Navigation Properties
        public virtual ShoppingSession Session { get; set; } = null!;
        public virtual ICollection<CompletedOrderItem> Items { get; set; } = [];
    }

}

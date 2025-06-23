using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace DBEntities.Models
{
    public class ShoppingSession
    {
        public Guid ShoppingSessionId { get; set; }

        [MaxLength(100)]
        public string? SessionName { get; set; }

        [MaxLength(20)]
        public string Status { get; set; } = "Active";

        public int TotalItems { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }

        [MaxLength(1000)]
        public string? Notes { get; set; }

        // Navigation Properties
        public virtual ICollection<ShoppingCartItem> CartItems { get; set; } = [];
        public virtual ICollection<CompletedOrder> CompletedOrders { get; set; } = [];
    }
}


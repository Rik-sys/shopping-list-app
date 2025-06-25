using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;


namespace DBEntities.Models
{
    public class ShoppingCartItem
    {
        public int ShoppingCartItemId { get; set; }
        public Guid SessionId { get; set; }

        [Required, MaxLength(150)]
        public string ProductName { get; set; } = string.Empty;

        public int CategoryId { get; set; }
        public int Quantity { get; set; } = 1;

        [MaxLength(20)]
        public string Unit { get; set; } = "יחידה";

        [MaxLength(10)]
        public string Priority { get; set; } = "Normal";

        public bool IsChecked { get; set; } = false;
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CheckedAt { get; set; }

        [MaxLength(255)]
        public string? Notes { get; set; }
        public virtual ShoppingSession Session { get; set; } = null!;
        public virtual Category Category { get; set; } = null!;
    }
}

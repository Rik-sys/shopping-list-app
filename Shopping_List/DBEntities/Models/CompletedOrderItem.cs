
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;


namespace DBEntities.Models
{
    public class CompletedOrderItem
    {
        public int CompletedOrderItemId { get; set; }
        public int OrderId { get; set; }

        [Required, MaxLength(150)]
        public string ProductName { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string CategoryName { get; set; } = string.Empty;

        public int CategoryId { get; set; }
        public int Quantity { get; set; }

        [MaxLength(20)]
        public string Unit { get; set; } = "יחידה";

        [MaxLength(10)]
        public string Priority { get; set; } = "Normal";

        public virtual CompletedOrder Order { get; set; } = null!;
        public virtual Category Category { get; set; } = null!;
    }

}

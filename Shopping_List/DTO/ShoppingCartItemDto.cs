using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ShoppingCartItemDto
    {
        public int Id { get; set; }
        public Guid SessionId { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }
        public string Priority { get; set; }
        public bool IsChecked { get; set; }
        public DateTime AddedAt { get; set; }
        public string? Notes { get; set; }

       
    }
}

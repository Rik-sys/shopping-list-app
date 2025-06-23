using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class AddItemRequestDto
    {
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        public int Quantity { get; set; } = 1;
        public string Unit { get; set; } = "יחידה";
        public string Priority { get; set; } = "Normal";
        public string? Notes { get; set; }

       
    }
}

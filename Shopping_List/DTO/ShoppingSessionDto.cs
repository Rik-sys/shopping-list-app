using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ShoppingSessionDto
    {
        public Guid SessionId { get; set; }
        public string? SessionName { get; set; }
        public string Status { get; set; } = "Active";
        public int TotalItems { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string? Notes { get; set; }
    }
}

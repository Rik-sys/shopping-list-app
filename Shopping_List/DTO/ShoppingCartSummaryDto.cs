using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ShoppingCartSummaryDto
    {
        public Guid SessionId { get; set; }
        public int TotalItems { get; set; }
        public int TotalCategories { get; set; }
        public List<CartCategoryDto> Categories { get; set; }
        public DateTime LastUpdated { get; set; }
        public string Status { get; set; } = "Active";
        public ShoppingCartSummaryDto() { }
        public ShoppingCartSummaryDto(Guid sessionId, int totalItems, int totalCategories, List<CartCategoryDto> categories, DateTime lastUpdated, string status = "Active")
        {
            SessionId = sessionId;
            TotalItems = totalItems;
            TotalCategories = totalCategories;
            Categories = categories;
            LastUpdated = lastUpdated;
            Status = status;
        }
    }
}

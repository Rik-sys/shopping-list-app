using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class CartCategoryDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string? CategoryIcon { get; set; }
        public int TotalItems { get; set; }
        public List<ShoppingCartItemDto> Items { get; set; }
        public CartCategoryDto() { }
        public CartCategoryDto(int categoryId, string categoryName, string? categoryIcon, int totalItems, List<ShoppingCartItemDto> items)
        {
            CategoryId = categoryId;
            CategoryName = categoryName;
            CategoryIcon = categoryIcon;
            TotalItems = totalItems;
            Items = items;
        }
    }
}

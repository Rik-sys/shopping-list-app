using DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IBL
{
    public interface IShoppingBLL
    {
        Task<List<CategoryDto>> GetAllCategoriesAsync();
        Task<ShoppingCartItemDto> AddItemToCartAsync(AddItemRequestDto request);
        Task<ShoppingCartSummaryDto> GetCurrentCartAsync(Guid sessionId);
        Task<CompletedOrderDto> CompleteOrderAsync(CompleteOrderRequestDto request); // תוקן פה
        Task<bool> UpdateItemQuantityAsync(UpdateItemRequestDto request);
        Task<bool> RemoveItemFromCartAsync(int itemId);
        Task<ShoppingSessionDto> GetOrCreateActiveSessionAsync();
    }
}

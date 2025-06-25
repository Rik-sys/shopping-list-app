using DBEntities.Models;

namespace IDAL
{
    public interface IShoppingDAL
    {
        Task<ShoppingSession> GetOrCreateActiveSessionAsync();
        Task<ShoppingSession?> GetSessionAsync(Guid shoppingSessionId); 
        Task<ShoppingSession> UpdateSessionAsync(ShoppingSession session);
        Task<List<ShoppingCartItem>> GetCartItemsAsync(Guid shoppingSessionId); 
        Task<ShoppingCartItem?> GetCartItemAsync(Guid shoppingSessionId, string productName, int categoryId); 
        Task<ShoppingCartItem> AddCartItemAsync(ShoppingCartItem item);
        Task<ShoppingCartItem> UpdateCartItemAsync(ShoppingCartItem item);
        Task<bool> RemoveCartItemAsync(int itemId);
        Task<bool> UpdateSessionTotalsAsync(Guid shoppingSessionId); 
        Task<CompletedOrder> CreateCompletedOrderAsync(CompletedOrder order);
        Task<bool> CreateOrderItemsAsync(List<CompletedOrderItem> items);
        Task<ShoppingCartItem?> GetCartItemByIdAsync(int cartItemId);
    }
}

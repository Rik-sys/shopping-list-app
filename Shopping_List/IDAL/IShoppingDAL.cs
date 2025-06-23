using DBEntities.Models;

namespace IDAL
{
    public interface IShoppingDAL
    {
        // Updated method signatures to match ShoppingSession.ShoppingSessionId
        Task<ShoppingSession> GetOrCreateActiveSessionAsync();
        Task<ShoppingSession?> GetSessionAsync(Guid shoppingSessionId); // Changed parameter name
        Task<ShoppingSession> UpdateSessionAsync(ShoppingSession session);
        Task<List<ShoppingCartItem>> GetCartItemsAsync(Guid shoppingSessionId); // Changed parameter name
        Task<ShoppingCartItem?> GetCartItemAsync(Guid shoppingSessionId, string productName, int categoryId); // Changed parameter name
        Task<ShoppingCartItem> AddCartItemAsync(ShoppingCartItem item);
        Task<ShoppingCartItem> UpdateCartItemAsync(ShoppingCartItem item);
        Task<bool> RemoveCartItemAsync(int itemId);
        Task<bool> UpdateSessionTotalsAsync(Guid shoppingSessionId); // Changed parameter name
        Task<CompletedOrder> CreateCompletedOrderAsync(CompletedOrder order);
        Task<bool> CreateOrderItemsAsync(List<CompletedOrderItem> items);

        // TODO: Add this method to support UpdateItemQuantityAsync
        Task<ShoppingCartItem?> GetCartItemByIdAsync(int cartItemId);
    }
}

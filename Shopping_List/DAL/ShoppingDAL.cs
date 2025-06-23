using DAL.ContextDir;
using DBEntities.Models;
using IDAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace DAL
{
    public class ShoppingDAL : IShoppingDAL
    {
        private readonly ShoppingListContext _context;
        private readonly ILogger<ShoppingDAL> _logger;

        public ShoppingDAL(ShoppingListContext context, ILogger<ShoppingDAL> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ShoppingSession> GetOrCreateActiveSessionAsync()
        {
            var activeSession = await _context.ShoppingSessions
                .FirstOrDefaultAsync(s => s.Status == "Active");

            if (activeSession == null)
            {
                activeSession = new ShoppingSession
                {
                    ShoppingSessionId = Guid.NewGuid(),
                    Status = "Active",
                    SessionName = $"קניות {DateTime.Now:dd/MM/yyyy HH:mm}",
                    CreatedAt = DateTime.UtcNow,
                    TotalItems = 0
                };

                _context.ShoppingSessions.Add(activeSession);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Created new shopping session: {SessionId}", activeSession.ShoppingSessionId);
            }

            return activeSession;
        }

        public async Task<ShoppingSession?> GetSessionAsync(Guid sessionId)
        {
            return await _context.ShoppingSessions
                .FirstOrDefaultAsync(s => s.ShoppingSessionId == sessionId);
        }

        public async Task<ShoppingSession> UpdateSessionAsync(ShoppingSession session)
        {
            _context.ShoppingSessions.Update(session);
            await _context.SaveChangesAsync();
            return session;
        }

        public async Task<List<ShoppingCartItem>> GetCartItemsAsync(Guid sessionId)
        {
            return await _context.ShoppingCartItems
                .Where(item => item.SessionId == sessionId)
                .Include(item => item.Category)
                .OrderBy(item => item.Category.SortOrder)
                .ThenBy(item => item.AddedAt)
                .ToListAsync();
        }

        public async Task<ShoppingCartItem?> GetCartItemAsync(Guid sessionId, string productName, int categoryId)
        {
            return await _context.ShoppingCartItems
                .Include(item => item.Category)
                .FirstOrDefaultAsync(item =>
                    item.SessionId == sessionId &&
                    item.ProductName.ToLower() == productName.ToLower() &&
                    item.CategoryId == categoryId);
        }

        public async Task<ShoppingCartItem> AddCartItemAsync(ShoppingCartItem item)
        {
            _context.ShoppingCartItems.Add(item);
            await _context.SaveChangesAsync();

            // Load the category for the item
            await _context.Entry(item)
                .Reference(i => i.Category)
                .LoadAsync();

            _logger.LogInformation("Added cart item: {ProductName} to session {SessionId}",
                item.ProductName, item.SessionId);

            return item;
        }

        public async Task<ShoppingCartItem> UpdateCartItemAsync(ShoppingCartItem item)
        {
            _context.ShoppingCartItems.Update(item);
            await _context.SaveChangesAsync();

            // Reload with category
            await _context.Entry(item)
                .Reference(i => i.Category)
                .LoadAsync();

            _logger.LogInformation("Updated cart item: {ItemId} quantity to {Quantity}",
                item.ShoppingCartItemId, item.Quantity);

            return item;
        }

        public async Task<bool> RemoveCartItemAsync(int itemId)
        {
            var item = await _context.ShoppingCartItems.FindAsync(itemId);
            if (item == null) return false;

            _context.ShoppingCartItems.Remove(item);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Removed cart item: {ItemId}", itemId);
            return true;
        }

        public async Task<bool> UpdateSessionTotalsAsync(Guid sessionId)
        {
            var session = await GetSessionAsync(sessionId);
            if (session == null) return false;

            var totalItems = await _context.ShoppingCartItems
                .Where(item => item.SessionId == sessionId)
                .SumAsync(item => item.Quantity);

            session.TotalItems = totalItems;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<CompletedOrder> CreateCompletedOrderAsync(CompletedOrder order)
        {
            _context.CompletedOrders.Add(order);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Created completed order: {OrderNumber}", order.OrderNumber);
            return order;
        }

        public async Task<bool> CreateOrderItemsAsync(List<CompletedOrderItem> items)
        {
            _context.CompletedOrderItems.AddRange(items);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Created {Count} order items", items.Count);
            return true;
        }

        public async Task<ShoppingCartItem?> GetCartItemByIdAsync(int cartItemId)
        {
            try
            {
                return await _context.ShoppingCartItems
                    .Include(item => item.Category)
                    .Include(item => item.Session)
                    .FirstOrDefaultAsync(item => item.ShoppingCartItemId == cartItemId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting cart item by ID: {CartItemId}", cartItemId);
                throw;
            }
        }
    }

}

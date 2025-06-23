using AutoMapper;
using DBEntities.Models;
using DTO;
using IBL;
using IDAL;
using Microsoft.Extensions.Logging;


namespace BLL
{

    public class ShoppingBLL : IShoppingBLL
    {
        private readonly IShoppingDAL _shoppingDAL;
        private readonly ICategoryDAL _categoryDAL;
        private readonly IMapper _mapper;
        private readonly ILogger<ShoppingBLL> _logger;

        public ShoppingBLL(
            IShoppingDAL shoppingDAL,
            ICategoryDAL categoryDAL,
            IMapper mapper,
            ILogger<ShoppingBLL> logger)
        {
            _shoppingDAL = shoppingDAL;
            _categoryDAL = categoryDAL;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<CategoryDto>> GetAllCategoriesAsync()
        {
            try
            {
                // DAL returns DbEntities, AutoMapper converts to DTOs
                var categories = await _categoryDAL.GetAllActiveAsync();
                return _mapper.Map<List<CategoryDto>>(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting categories");
                throw new BusinessException("שגיאה בטעינת הקטגוריות");
            }
        }

        public async Task<ShoppingCartItemDto> AddItemToCartAsync(AddItemRequestDto request)
        {
            try
            {
                // Business Logic Validation
                ValidateAddItemRequest(request);

                // Check if category exists - DAL returns Entity
                var category = await _categoryDAL.GetByIdAsync(request.CategoryId);
                if (category == null)
                    throw new BusinessException("הקטגוריה שנבחרה לא קיימת");

                // Get or create shopping session - DAL returns Entity
                var session = await _shoppingDAL.GetOrCreateActiveSessionAsync();

                // Check if item already exists - DAL returns Entity
                // FIX: Use ShoppingSessionId instead of SessionId
                var existingItem = await _shoppingDAL.GetCartItemAsync(
                    session.ShoppingSessionId, request.ProductName, request.CategoryId);

                ShoppingCartItem cartItem;
                if (existingItem != null)
                {
                    // Update existing item quantity
                    existingItem.Quantity += request.Quantity;
                    cartItem = await _shoppingDAL.UpdateCartItemAsync(existingItem);
                }
                else
                {
                    // Create new cart item Entity from DTO using AutoMapper
                    cartItem = _mapper.Map<ShoppingCartItem>(request);
                    cartItem.SessionId = session.ShoppingSessionId; // FIX: Use ShoppingSessionId
                    cartItem.AddedAt = DateTime.UtcNow;
                    cartItem = await _shoppingDAL.AddCartItemAsync(cartItem);
                }

                // Update session totals
                // FIX: Use ShoppingSessionId instead of SessionId
                await _shoppingDAL.UpdateSessionTotalsAsync(session.ShoppingSessionId);

                // Convert Entity back to DTO using AutoMapper
                var result = _mapper.Map<ShoppingCartItemDto>(cartItem);

                // FIX: Update CategoryName properly (can't use 'with' on class)
                result.CategoryName = category.Name;

                return result;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding item to cart");
                throw new BusinessException("שגיאה בהוספת המוצר לסל");
            }
        }

        public async Task<ShoppingCartSummaryDto> GetCurrentCartAsync(Guid sessionId)
        {
            try
            {
                // DAL returns Entities, AutoMapper converts to DTOs
                var session = await _shoppingDAL.GetSessionAsync(sessionId);
                if (session == null)
                    throw new BusinessException("סשן הקניות לא נמצא");

                var cartItems = await _shoppingDAL.GetCartItemsAsync(sessionId);
                var categories = await _categoryDAL.GetAllActiveAsync();

                // Convert Entities to DTOs using AutoMapper
                var cartItemDtos = _mapper.Map<List<ShoppingCartItemDto>>(cartItems);
                var categoryDtos = _mapper.Map<List<CategoryDto>>(categories);

                // Set category names for cart items
                // FIX: Create new list instead of modifying foreach variable
                var updatedCartItems = new List<ShoppingCartItemDto>();
                foreach (var item in cartItemDtos)
                {
                    var category = categoryDtos.First(c => c.Id == item.CategoryId);
                    item.CategoryName = category.Name; // This works because DTOs are classes
                    updatedCartItems.Add(item);
                }

                // Group items by category - Pure DTO operations
                var groupedItems = updatedCartItems
                    .GroupBy(item => item.CategoryId)
                    .Select(group =>
                    {
                        var category = categoryDtos.First(c => c.Id == group.Key);
                        // FIX: Use proper constructor parameters
                        return new CartCategoryDto(
                            categoryId: category.Id,
                            categoryName: category.Name,
                            categoryIcon: category.IconName,
                            totalItems: group.Sum(i => i.Quantity),
                            items: group.ToList()
                        );
                    })
                    .OrderBy(c => categoryDtos.First(cat => cat.Id == c.CategoryId).SortOrder)
                    .ToList();

                // FIX: Use proper constructor parameters
                return new ShoppingCartSummaryDto(
                    sessionId: sessionId,
                    totalItems: updatedCartItems.Sum(i => i.Quantity),
                    totalCategories: groupedItems.Count,
                    categories: groupedItems,
                    lastUpdated: updatedCartItems.Any() ? updatedCartItems.Max(i => i.AddedAt) : session.CreatedAt
                );
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting current cart");
                throw new BusinessException("שגיאה בטעינת סל הקניות");
            }
        }

        // FIX: Return type should be CompletedOrderDto, not CompleteOrderRequestDto
        public async Task<CompletedOrderDto> CompleteOrderAsync(CompleteOrderRequestDto request)
        {
            try
            {
                // DAL returns Entities
                var session = await _shoppingDAL.GetSessionAsync(request.SessionId);
                if (session == null)
                    throw new BusinessException("סשן הקניות לא נמצא");

                var cartItems = await _shoppingDAL.GetCartItemsAsync(request.SessionId);
                if (!cartItems.Any())
                    throw new BusinessException("הסל ריק - אין מוצרים להזמנה");

                // Generate order number
                var orderNumber = GenerateOrderNumber();

                // Create completed order Entity
                var completedOrder = new CompletedOrder
                {
                    OrderNumber = orderNumber,
                    SessionId = request.SessionId,
                    TotalItems = cartItems.Sum(i => i.Quantity),
                    TotalCategories = cartItems.Select(i => i.CategoryId).Distinct().Count(),
                    CompletedAt = DateTime.UtcNow,
                    Notes = request.Notes
                };

                var savedOrder = await _shoppingDAL.CreateCompletedOrderAsync(completedOrder);

                // Create order items Entities using AutoMapper
                var categories = await _categoryDAL.GetAllActiveAsync();
                var orderItems = cartItems.Select(cartItem =>
                {
                    var orderItem = _mapper.Map<CompletedOrderItem>(cartItem);
                    orderItem.OrderId = savedOrder.CompletedOrderId;
                    orderItem.CategoryName = categories.First(c => c.CategoryId == cartItem.CategoryId).Name;
                    return orderItem;
                }).ToList();

                await _shoppingDAL.CreateOrderItemsAsync(orderItems);

                // Update session status
                session.Status = "Completed";
                session.CompletedAt = DateTime.UtcNow;
                await _shoppingDAL.UpdateSessionAsync(session);

                // Convert to DTO using AutoMapper
                var result = _mapper.Map<CompletedOrderDto>(savedOrder);
                result.Items = _mapper.Map<List<CompletedOrderItemDto>>(orderItems);

                return result;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error completing order");
                throw new BusinessException("שגיאה בהשלמת ההזמנה");
            }
        }

        public async Task<bool> UpdateItemQuantityAsync(UpdateItemRequestDto request)
        {
            try
            {
                if (request.Quantity.HasValue && request.Quantity <= 0)
                    throw new BusinessException("כמות המוצר חייבת להיות גדולה מ-0");

                // Get the cart item by ID
                var item = await _shoppingDAL.GetCartItemByIdAsync(request.ItemId);
                if (item == null)
                    throw new BusinessException("המוצר לא נמצא בסל");

                // Update the item properties
                if (request.Quantity.HasValue)
                    item.Quantity = request.Quantity.Value;

                if (request.IsChecked.HasValue)
                {
                    item.IsChecked = request.IsChecked.Value;
                    item.CheckedAt = request.IsChecked.Value ? DateTime.UtcNow : null;
                }

                if (!string.IsNullOrEmpty(request.Notes))
                    item.Notes = request.Notes;

                // Update the item in database
                await _shoppingDAL.UpdateCartItemAsync(item);

                // Update session totals
                await _shoppingDAL.UpdateSessionTotalsAsync(item.SessionId);

                _logger.LogInformation("Updated cart item {ItemId} - Quantity: {Quantity}, IsChecked: {IsChecked}",
                    request.ItemId, request.Quantity, request.IsChecked);

                return true;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating item quantity for item {ItemId}", request.ItemId);
                throw new BusinessException("שגיאה בעדכון המוצר");
            }
        }

        public async Task<bool> RemoveItemFromCartAsync(int itemId)
        {
            try
            {
                var success = await _shoppingDAL.RemoveCartItemAsync(itemId);
                if (!success)
                    throw new BusinessException("המוצר לא נמצא בסל");

                return true;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing item from cart");
                throw new BusinessException("שגיאה בהסרת המוצר מהסל");
            }
        }

        // Private Helper Methods
        private static void ValidateAddItemRequest(AddItemRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.ProductName))
                throw new BusinessException("שם המוצר הוא שדה חובה");

            if (request.CategoryId <= 0)
                throw new BusinessException("יש לבחור קטגוריה");

            if (request.Quantity <= 0)
                throw new BusinessException("כמות המוצר חייבת להיות גדולה מ-0");
        }

        private static string GenerateOrderNumber()
        {
            return $"ORD-{DateTime.Now:yyyyMMdd}-{new Random().Next(1000, 9999)}";
        }
        public async Task<ShoppingSessionDto> GetOrCreateActiveSessionAsync()
        {
            try
            {
                var session = await _shoppingDAL.GetOrCreateActiveSessionAsync();

                // המרה ידנית (בלי AutoMapper כי זה פשוט)
                return new ShoppingSessionDto
                {
                    SessionId = session.ShoppingSessionId,
                    SessionName = session.SessionName,
                    Status = session.Status,
                    TotalItems = session.TotalItems,
                    CreatedAt = session.CreatedAt,
                    CompletedAt = session.CompletedAt,
                    Notes = session.Notes
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating or getting active session");
                throw new BusinessException("שגיאה ביצירת סשן קניות");
            }
        }
    }

}

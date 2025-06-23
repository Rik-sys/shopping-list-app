using Microsoft.AspNetCore.Mvc;
using DTO;
using IBL;
using BLL;

namespace ShoppingList.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShoppingController : ControllerBase
    {
        private readonly IShoppingBLL _shoppingBLL;
        private readonly ILogger<ShoppingController> _logger;

        public ShoppingController(IShoppingBLL shoppingBLL, ILogger<ShoppingController> logger)
        {
            _shoppingBLL = shoppingBLL;
            _logger = logger;
        }

        /// <summary>
        /// Get all active categories
        /// </summary>
        [HttpGet("categories")]
        [ProducesResponseType(typeof(ApiResponse<List<CategoryDto>>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        public async Task<ActionResult<ApiResponse<List<CategoryDto>>>> GetCategories()
        {
            try
            {
                var categories = await _shoppingBLL.GetAllCategoriesAsync();
                return Ok(new ApiResponse<List<CategoryDto>>(categories, "קטגוריות נטענו בהצלחה"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting categories");
                return StatusCode(500, new ApiResponse<List<CategoryDto>>("שגיאת שרת", "INTERNAL_ERROR"));
            }
        }

        /// <summary>
        /// Add item to shopping cart
        /// </summary>
        [HttpPost("add-item")]
        [ProducesResponseType(typeof(ApiResponse<ShoppingCartItemDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        public async Task<ActionResult<ApiResponse<ShoppingCartItemDto>>> AddItem([FromBody] AddItemRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));
                    return BadRequest(new ApiResponse<ShoppingCartItemDto>($"נתונים שגויים: {errors}", "VALIDATION_ERROR"));
                }

                var result = await _shoppingBLL.AddItemToCartAsync(request);
                return Ok(new ApiResponse<ShoppingCartItemDto>(result, "המוצר נוסף בהצלחה"));
            }
            catch (BusinessException ex)
            {
                return BadRequest(new ApiResponse<ShoppingCartItemDto>(ex.Message, "BUSINESS_ERROR"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding item to cart");
                return StatusCode(500, new ApiResponse<ShoppingCartItemDto>("שגיאת שרת", "INTERNAL_ERROR"));
            }
        }

        /// <summary>
        /// Get current shopping cart
        /// </summary>
        [HttpGet("current-cart/{sessionId:guid}")]
        [ProducesResponseType(typeof(ApiResponse<ShoppingCartSummaryDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        public async Task<ActionResult<ApiResponse<ShoppingCartSummaryDto>>> GetCurrentCart(Guid sessionId)
        {
            try
            {
                if (sessionId == Guid.Empty)
                {
                    return BadRequest(new ApiResponse<ShoppingCartSummaryDto>("מזהה סשן לא תקין", "INVALID_SESSION"));
                }

                var result = await _shoppingBLL.GetCurrentCartAsync(sessionId);
                return Ok(new ApiResponse<ShoppingCartSummaryDto>(result, "סל הקניות נטען בהצלחה"));
            }
            catch (BusinessException ex)
            {
                return BadRequest(new ApiResponse<ShoppingCartSummaryDto>(ex.Message, "BUSINESS_ERROR"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting current cart for session {SessionId}", sessionId);
                return StatusCode(500, new ApiResponse<ShoppingCartSummaryDto>("שגיאת שרת", "INTERNAL_ERROR"));
            }
        }

        /// <summary>
        /// Complete the current order
        /// </summary>
        [HttpPost("complete-order")]
        [ProducesResponseType(typeof(ApiResponse<CompletedOrderDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        public async Task<ActionResult<ApiResponse<CompletedOrderDto>>> CompleteOrder([FromBody] CompleteOrderRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));
                    return BadRequest(new ApiResponse<CompletedOrderDto>($"נתונים שגויים: {errors}", "VALIDATION_ERROR"));
                }

                var result = await _shoppingBLL.CompleteOrderAsync(request);
                return Ok(new ApiResponse<CompletedOrderDto>(result, "ההזמנה הושלמה בהצלחה"));
            }
            catch (BusinessException ex)
            {
                return BadRequest(new ApiResponse<CompletedOrderDto>(ex.Message, "BUSINESS_ERROR"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error completing order for session {SessionId}", request.SessionId);
                return StatusCode(500, new ApiResponse<CompletedOrderDto>("שגיאת שרת", "INTERNAL_ERROR"));
            }
        }

        /// <summary>
        /// Update item quantity or check status
        /// </summary>
        [HttpPut("update-item")]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateItem([FromBody] UpdateItemRequestDto request)
        {
            try
            {
                var result = await _shoppingBLL.UpdateItemQuantityAsync(request);
                return Ok(new ApiResponse<bool>(result, "המוצר עודכן בהצלחה"));
            }
            catch (BusinessException ex)
            {
                return BadRequest(new ApiResponse<bool>(ex.Message, "BUSINESS_ERROR"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating item {ItemId}", request.ItemId);
                return StatusCode(500, new ApiResponse<bool>("שגיאת שרת", "INTERNAL_ERROR"));
            }
        }

        /// <summary>
        /// Remove item from cart
        /// </summary>
        [HttpDelete("remove-item/{itemId:int}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        public async Task<ActionResult<ApiResponse<bool>>> RemoveItem(int itemId)
        {
            try
            {
                if (itemId <= 0)
                {
                    return BadRequest(new ApiResponse<bool>("מזהה פריט לא תקין", "INVALID_ITEM_ID"));
                }

                var result = await _shoppingBLL.RemoveItemFromCartAsync(itemId);
                return Ok(new ApiResponse<bool>(result, "המוצר הוסר בהצלחה"));
            }
            catch (BusinessException ex)
            {
                return BadRequest(new ApiResponse<bool>(ex.Message, "BUSINESS_ERROR"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing item {ItemId}", itemId);
                return StatusCode(500, new ApiResponse<bool>("שגיאת שרת", "INTERNAL_ERROR"));
            }
        }

        /// <summary>
        /// Create new shopping session
        /// </summary>
        [HttpPost("new-session")]
        [ProducesResponseType(typeof(ApiResponse<Guid>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        public async Task<ActionResult<ApiResponse<Guid>>> CreateNewSession()
        {
            try
            {
                // השתמש ב-BLL ליצירת סשן אמיתי
                var session = await _shoppingBLL.GetOrCreateActiveSessionAsync();
                return Ok(new ApiResponse<Guid>(session.SessionId, "סשן חדש נוצר בהצלחה"));
            }
            catch (BusinessException ex)
            {
                return BadRequest(new ApiResponse<Guid>(ex.Message, "BUSINESS_ERROR"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating new session");
                return StatusCode(500, new ApiResponse<Guid>("שגיאת שרת", "INTERNAL_ERROR"));
            }
        }

    }
}
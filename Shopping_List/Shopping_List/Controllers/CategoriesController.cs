using DTO;
using IBL;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryBLL _categoryBLL;
    private readonly ILogger<CategoriesController> _logger;

    public CategoriesController(ICategoryBLL categoryBLL, ILogger<CategoriesController> logger)
    {
        _categoryBLL = categoryBLL;
        _logger = logger;
    }

    /// <summary>
    /// Get all categories
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<List<CategoryDto>>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 500)]
    public async Task<ActionResult<ApiResponse<List<CategoryDto>>>> GetCategories()
    {
        try
        {
            var categories = await _categoryBLL.GetAllCategoriesAsync();
            return Ok(new ApiResponse<List<CategoryDto>>(categories, "קטגוריות נטענו בהצלחה"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting categories");
            return StatusCode(500, new ApiResponse<List<CategoryDto>>("שגיאת שרת", "INTERNAL_ERROR"));
        }
    }

    /// <summary>
    /// Get category by ID
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<CategoryDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 404)]
    [ProducesResponseType(typeof(ApiResponse<object>), 500)]
    public async Task<ActionResult<ApiResponse<CategoryDto>>> GetCategory(int id)
    {
        try
        {
            var category = await _categoryBLL.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound(new ApiResponse<CategoryDto>("הקטגוריה לא נמצאה", "CATEGORY_NOT_FOUND"));
            }

            return Ok(new ApiResponse<CategoryDto>(category, "קטגוריה נטענה בהצלחה"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting category {CategoryId}", id);
            return StatusCode(500, new ApiResponse<CategoryDto>("שגיאת שרת", "INTERNAL_ERROR"));
        }
    }
}

using AutoMapper;
using DTO;
using IBL;
using IDAL;
using Microsoft.Extensions.Logging;


namespace BLL
{
    public class CategoryBLL : ICategoryBLL
    {
        private readonly ICategoryDAL _categoryDAL;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryBLL> _logger;

        public CategoryBLL(ICategoryDAL categoryDAL, IMapper mapper, ILogger<CategoryBLL> logger)
        {
            _categoryDAL = categoryDAL;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<CategoryDto>> GetAllCategoriesAsync()
        {
            try
            {
                var categories = await _categoryDAL.GetAllActiveAsync();
                return _mapper.Map<List<CategoryDto>>(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting categories");
                throw new BusinessException("שגיאה בטעינת הקטגוריות");
            }
        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(int categoryId)
        {
            try
            {
                var category = await _categoryDAL.GetByIdAsync(categoryId);
                return category == null ? null : _mapper.Map<CategoryDto>(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting category {CategoryId}", categoryId);
                throw new BusinessException("שגיאה בטעינת הקטגוריה");
            }
        }
    }
}

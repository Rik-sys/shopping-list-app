using DTO;


namespace IBL
{
    public interface ICategoryBLL
    {
        Task<List<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto?> GetCategoryByIdAsync(int categoryId);
    }
}

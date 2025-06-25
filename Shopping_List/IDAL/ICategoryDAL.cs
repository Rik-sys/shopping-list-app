using DBEntities.Models;


namespace IDAL
{
    public interface ICategoryDAL
    {
        Task<List<Category>> GetAllActiveAsync();
        Task<Category?> GetByIdAsync(int categoryId);
        Task<Category> CreateAsync(Category category);
        Task<Category> UpdateAsync(Category category);
        Task<bool> DeleteAsync(int categoryId);
    }
}

using DAL.ContextDir;
using DBEntities.Models;
using IDAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace DAL
{
    public class CategoryDAL : ICategoryDAL
    {
        private readonly ShoppingListContext _context;
        private readonly ILogger<CategoryDAL> _logger;

        public CategoryDAL(ShoppingListContext context, ILogger<CategoryDAL> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Category>> GetAllActiveAsync()
        {
            return await _context.Categories
                .Where(c => c.IsActive)
                .OrderBy(c => c.SortOrder)
                .ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int categoryId)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryId == categoryId && c.IsActive);
        }

        public async Task<Category> CreateAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<bool> DeleteAsync(int categoryId)
        {
            var category = await GetByIdAsync(categoryId);
            if (category == null) return false;

            category.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

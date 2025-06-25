

namespace DTO
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? IconName { get; set; }
        public int SortOrder { get; set; } = 0;
        public bool IsActive { get; set; } = true;

      
    }
}

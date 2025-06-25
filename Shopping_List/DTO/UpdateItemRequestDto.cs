
namespace DTO
{
    public class UpdateItemRequestDto
    {
        public int ItemId { get; set; }
        public int? Quantity { get; set; }
        public bool? IsChecked { get; set; }
        public string? Notes { get; set; }

       
    }
}

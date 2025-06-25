
namespace DTO
{
    public class CompletedOrderDto
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public int TotalItems { get; set; }
        public int TotalCategories { get; set; }
        public DateTime CompletedAt { get; set; }
        public string? Notes { get; set; }
        public List<CompletedOrderItemDto> Items { get; set; }

        

        
        public CompletedOrderDto()
        {
            OrderNumber = string.Empty;
            Items = new List<CompletedOrderItemDto>();
        }
    }
}

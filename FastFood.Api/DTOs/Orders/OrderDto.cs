namespace FastFood.Api.DTOs.Orders
{
    public class OrderDto
    {
        public int Id { get; set; }

        public decimal TotalPrice { get; set; }

        public string Status { get; set; } = string.Empty;

        public List<OrderItemDto> Items { get; set; }
            = new List<OrderItemDto>();
    }
}
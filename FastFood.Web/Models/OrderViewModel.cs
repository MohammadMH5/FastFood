namespace FastFood.Web.Models
{
    public class OrderViewModel
    {
        public int Id { get; set; }

        public decimal TotalPrice { get; set; }

        public string Status { get; set; } = string.Empty;

        public List<OrderItemViewModel> Items { get; set; } = new();
    }

    public class OrderItemViewModel
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }
    }
}

namespace FastFood.Api.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;

        public decimal TotalPrice { get; set; }

        public string Status { get; set; } = "Pending";

        public ICollection<OrderItem> OrderItems { get; set; }
            = new List<OrderItem>();
    }
}
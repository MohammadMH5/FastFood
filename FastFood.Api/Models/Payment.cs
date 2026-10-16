namespace FastFood.Api.Models
{
    public class Payment
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public Order Order { get; set; } = null!;

        public decimal Amount { get; set; }

        public string Status { get; set; } = "Pending";

        public string? TrackingCode { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
namespace FastFood.Api.DTOs.Payments
{
    public class PaymentDto
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public decimal Amount { get; set; }

        public string Status { get; set; } = string.Empty;

        public string? TrackingCode { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
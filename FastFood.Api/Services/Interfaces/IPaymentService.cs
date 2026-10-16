using FastFood.Api.DTOs.Payments;

namespace FastFood.Api.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentDto?> CreatePaymentAsync(
            int orderId,
            int userId);

        Task<PaymentDto?> GetPaymentByOrderIdAsync(
            int orderId,
            int userId);
    }
}
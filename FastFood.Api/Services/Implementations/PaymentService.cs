using FastFood.Api.DTOs.Payments;
using FastFood.Api.Exceptions;
using FastFood.Api.Models;
using FastFood.Api.Services.Interfaces;
using FastFood.Api.UnitOfWork;

namespace FastFood.Api.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaymentDto?> CreatePaymentAsync(
            int orderId,
            int userId)
        {
            var order =
                await _unitOfWork.Orders
                    .GetByIdWithItemsAsync(orderId);

            if (order == null)
            {
                return null;
            }

            if (order.UserId != userId)
            {
                return null;
            }

            var existingPayment =
                await _unitOfWork.Payments
                    .GetByOrderIdAsync(orderId);

            if (existingPayment != null)
            {
                throw new BusinessException(
                    "برای این سفارش قبلاً پرداخت ایجاد شده است.");
            }

            var payment = new Payment
            {
                OrderId = order.Id,
                Amount = order.TotalPrice,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Payments
                .AddAsync(payment);

            await _unitOfWork.SaveChangesAsync();

            return MapToDto(payment);
        }

        public async Task<PaymentDto?> GetPaymentByOrderIdAsync(
            int orderId,
            int userId)
        {
            var order =
                await _unitOfWork.Orders
                    .GetByIdWithItemsAsync(orderId);

            if (order == null)
            {
                return null;
            }

            if (order.UserId != userId)
            {
                return null;
            }

            var payment =
                await _unitOfWork.Payments
                    .GetByOrderIdAsync(orderId);

            if (payment == null)
            {
                return null;
            }

            return MapToDto(payment);
        }

        private static PaymentDto MapToDto(
            Payment payment)
        {
            return new PaymentDto
            {
                Id = payment.Id,
                OrderId = payment.OrderId,
                Amount = payment.Amount,
                Status = payment.Status,
                TrackingCode = payment.TrackingCode,
                CreatedAt = payment.CreatedAt
            };
        }
    }
}

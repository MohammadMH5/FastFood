using FastFood.Api.Models;

namespace FastFood.Api.Repositories
{
    public interface IPaymentRepository
        : IGenericRepository<Payment>
    {
        Task<Payment?> GetByOrderIdAsync(int orderId);
    }
}
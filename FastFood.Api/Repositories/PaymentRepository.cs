using FastFood.Api.Data;
using FastFood.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Api.Repositories
{
    public class PaymentRepository
        : GenericRepository<Payment>, IPaymentRepository
    {
        private readonly ApplicationDbContext _context;

        public PaymentRepository(
            ApplicationDbContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<Payment?> GetByOrderIdAsync(
            int orderId)
        {
            return await _context.Payments
                .FirstOrDefaultAsync(
                    p => p.OrderId == orderId);
        }
    }
}
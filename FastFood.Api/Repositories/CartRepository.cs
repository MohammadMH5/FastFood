using FastFood.Api.Data;
using FastFood.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Api.Repositories
{
    public class CartRepository
        : GenericRepository<Cart>, ICartRepository
    {
        private readonly ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<Cart?> GetCartByUserIdAsync(int userId)
        {
            return await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public Task RemoveItemAsync(CartItem cartItem)
        {
            _context.CartItems.Remove(cartItem);

            return Task.CompletedTask;
        }

        public Task ClearItemsAsync(Cart cart)
        {
            _context.CartItems.RemoveRange(cart.CartItems);

            return Task.CompletedTask;
        }
    }
}
using FastFood.Api.Models;

namespace FastFood.Api.Repositories
{
    public interface ICartRepository
        : IGenericRepository<Cart>
    {
        Task<Cart?> GetCartByUserIdAsync(int userId);

        Task RemoveItemAsync(CartItem cartItem);

        Task ClearItemsAsync(Cart cart);
    }
}
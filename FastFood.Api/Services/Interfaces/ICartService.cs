using FastFood.Api.DTOs.Carts;
using FastFood.Api.Models;

namespace FastFood.Api.Services.Interfaces
{
    public interface ICartService
    {
        Task<CartDto?> GetCartAsync(int userId);

        Task<Cart> CreateCartAsync(int userId);

        Task<bool> AddItemAsync(
            int userId,
            int productId,
            int quantity);

        Task<bool> UpdateItemAsync(
            int userId,
            int productId,
            int quantity);

        Task<bool> RemoveItemAsync(
            int userId,
            int productId);

        Task<bool> ClearCartAsync(
            int userId);
    }
}
using FastFood.Api.DTOs.Orders;

namespace FastFood.Api.Services.Interfaces
{
    public interface IOrderService
    {
        // Customer
        Task<OrderDto?> CreateOrderFromCartAsync(int userId);

        Task<IEnumerable<OrderDto>> GetMyOrdersAsync(
            int userId);

        Task<OrderDto?> GetOrderByIdAsync(
            int orderId,
            int userId);

        // Admin
        Task<IEnumerable<OrderDto>> GetAllOrdersAsync();

        Task<OrderDto?> GetOrderByIdForAdminAsync(
            int orderId);

        Task<bool> UpdateOrderStatusAsync(
            int orderId,
            string status);
    }
}
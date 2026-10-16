using FastFood.Api.Models;

namespace FastFood.Api.Repositories
{
    public interface IOrderRepository
        : IGenericRepository<Order>
    {
        Task<Order?> GetByIdWithItemsAsync(int id);

        Task<IEnumerable<Order>> GetAllWithItemsAsync();

        Task<IEnumerable<Order>> GetByUserIdAsync(int userId);
    }
}
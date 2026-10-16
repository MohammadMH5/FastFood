using FastFood.Api.Models;
using FastFood.Api.Repositories;

namespace FastFood.Api.UnitOfWork
{
    public interface IUnitOfWork
    {
        IProductRepository Products { get; }

        IGenericRepository<Category> Categories { get; }

        ICartRepository Carts { get; }
        IOrderRepository Orders { get; }
        IPaymentRepository Payments { get; }

        Task<int> SaveChangesAsync();
    }
}
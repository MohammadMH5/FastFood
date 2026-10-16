using FastFood.Api.Models;

namespace FastFood.Api.Repositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> GetAllWithCategoryAsync();

        Task<Product?> GetByIdWithCategoryAsync(int id);
    }
}
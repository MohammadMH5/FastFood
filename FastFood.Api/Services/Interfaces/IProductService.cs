using FastFood.Api.DTOs.Products;

namespace FastFood.Api.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllAsync();

        Task<ProductDto?> GetByIdAsync(int id);

        Task<ProductDto> AddAsync(CreateProductDto dto);

        Task<bool> UpdateAsync(int id, UpdateProductDto dto);

        Task<bool> UpdateImageUrlAsync(int id, string imageUrl);

        Task<bool> DeleteAsync(int id);
    }
}
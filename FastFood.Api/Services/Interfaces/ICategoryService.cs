using FastFood.Api.DTOs.Categories;

namespace FastFood.Api.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync();

        Task<CategoryDto?> GetByIdAsync(int id);

        Task<CategoryDto> AddAsync(CreateCategoryDto dto);

        Task<bool> UpdateAsync(int id, UpdateCategoryDto dto);

        Task<bool> DeleteAsync(int id);
    }
}
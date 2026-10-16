using FastFood.Api.DTOs.Categories;
using FastFood.Api.Models;
using FastFood.Api.Services.Interfaces;
using FastFood.Api.UnitOfWork;

namespace FastFood.Api.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();

            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            });
        }

        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);

            if (category == null)
                return null;

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
        }

        public async Task<CategoryDto> AddAsync(CreateCategoryDto dto)
        {
            var category = new Category
            {
                Name = dto.Name,
                Description = dto.Description
            };

            await _unitOfWork.Categories.AddAsync(category);

            await _unitOfWork.SaveChangesAsync();
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
        }

        public async Task<bool> UpdateAsync(int id, UpdateCategoryDto dto)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);

            if (category == null)
                return false;

            category.Name = dto.Name;
            category.Description = dto.Description;

            _unitOfWork.Categories.Update(category);

            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);

            if (category == null)
                return false;

            _unitOfWork.Categories.Delete(category);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
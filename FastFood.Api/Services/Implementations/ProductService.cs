using FastFood.Api.DTOs.Products;
using FastFood.Api.Exceptions;
using FastFood.Api.Models;
using FastFood.Api.Services.Interfaces;
using FastFood.Api.UnitOfWork;

namespace FastFood.Api.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var products =
                await _unitOfWork.Products.GetAllWithCategoryAsync();

            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                IsAvailable = p.IsAvailable,
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.Name ?? string.Empty
            });
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var product =
                await _unitOfWork.Products.GetByIdWithCategoryAsync(id);

            if (product == null)
                return null;

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                IsAvailable = product.IsAvailable,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name ?? string.Empty
            };
        }

        public async Task<ProductDto> AddAsync(CreateProductDto dto)
        {
            var category =
                await _unitOfWork.Categories.GetByIdAsync(dto.CategoryId);

            if (category == null)
            {
                throw new BusinessException(
                    "دسته‌بندی انتخاب شده وجود ندارد.");
            }

            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                IsAvailable = dto.IsAvailable,
                CategoryId = dto.CategoryId
            };

            await _unitOfWork.Products.AddAsync(product);

            await _unitOfWork.SaveChangesAsync();

            var createdProduct =
                await _unitOfWork.Products.GetByIdWithCategoryAsync(
                    product.Id);

            return new ProductDto
            {
                Id = createdProduct!.Id,
                Name = createdProduct.Name,
                Description = createdProduct.Description,
                Price = createdProduct.Price,
                ImageUrl = createdProduct.ImageUrl,
                IsAvailable = createdProduct.IsAvailable,
                CategoryId = createdProduct.CategoryId,
                CategoryName =
                    createdProduct.Category?.Name ?? string.Empty
            };
        }

        public async Task<bool> UpdateAsync(
            int id,
            UpdateProductDto dto)
        {
            var product =
                await _unitOfWork.Products.GetByIdAsync(id);

            if (product == null)
                return false;

            var category =
                await _unitOfWork.Categories.GetByIdAsync(dto.CategoryId);

            if (category == null)
            {
                throw new BusinessException(
                    "دسته‌بندی انتخاب شده وجود ندارد.");
            }

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.IsAvailable = dto.IsAvailable;
            product.CategoryId = dto.CategoryId;

            _unitOfWork.Products.Update(product);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateImageUrlAsync(
            int id,
            string imageUrl)
        {
            var product =
                await _unitOfWork.Products.GetByIdAsync(id);

            if (product == null)
                return false;

            product.ImageUrl = imageUrl;

            _unitOfWork.Products.Update(product);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product =
                await _unitOfWork.Products.GetByIdAsync(id);

            if (product == null)
                return false;

            _unitOfWork.Products.Delete(product);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}

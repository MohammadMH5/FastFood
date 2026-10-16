using FastFood.Api.DTOs.Products;
using FastFood.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FastFood.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll()
        {
            var products = await _productService.GetAllAsync();

            return Ok(products);
        }

        // GET: api/products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetById(int id)
        {
            var product = await _productService.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // POST: api/products
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProductDto>> Create(
            CreateProductDto dto)
        {
            var createdProduct = await _productService.AddAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdProduct.Id },
                createdProduct
            );
        }

        // POST: api/products/5/image
        [HttpPost("{id}/image")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UploadImage(
            int id,
            IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("فایل تصویر ارسال نشده است.");
            }
            var allowedExtensions = new[]
{
    ".jpg",
    ".jpeg",
    ".png",
    ".webp"
};

            var extension =
                Path.GetExtension(file.FileName)
                    .ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
            {
                return BadRequest(
                    "فرمت تصویر مجاز نیست. فقط JPG, JPEG, PNG و WEBP مجاز هستند.");
            }

            const long maxFileSize = 2 * 1024 * 1024;

            if (file.Length > maxFileSize)
            {
                return BadRequest(
                    "حجم تصویر نباید بیشتر از 2 مگابایت باشد.");
            }

            var product = await _productService.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            var uploadsFolder = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "images",
                "products");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fileName =
                $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            var filePath = Path.Combine(
                uploadsFolder,
                fileName);

            using (var stream = new FileStream(
                filePath,
                FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var imageUrl = $"/images/products/{fileName}";

            var updated =
                await _productService.UpdateImageUrlAsync(
                    id,
                    imageUrl);

            if (!updated)
            {
                return NotFound();
            }

            return Ok(new
            {
                message = "تصویر محصول با موفقیت آپلود شد.",
                imageUrl = imageUrl
            });
        }

        // PUT: api/products/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(
            int id,
            UpdateProductDto dto)
        {
            var product = await _productService.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            await _productService.UpdateAsync(id, dto);

            return NoContent();
        }

        // DELETE: api/products/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _productService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
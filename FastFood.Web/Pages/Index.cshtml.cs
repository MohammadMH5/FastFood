using FastFood.Web.Models;
using FastFood.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FastFood.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ProductApiService _productApiService;
        private readonly CategoryApiService _categoryApiService;
        private readonly CartService _cartService;

        public List<ProductDto> Products { get; set; }
            = new List<ProductDto>();

        public List<CategoryDto> Categories { get; set; }
            = new List<CategoryDto>();

        public IndexModel(
            ProductApiService productApiService,
            CategoryApiService categoryApiService,
            CartService cartService)
        {
            _productApiService = productApiService;
            _categoryApiService = categoryApiService;
            _cartService = cartService;
        }

        public async Task OnGetAsync()
        {
            Products = await _productApiService.GetProductsAsync();

            Categories = await _categoryApiService.GetCategoriesAsync();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(int productId)
        {
            // محصول را دوباره از API دریافت می‌کنیم؛
            // چون هنگام POST، لیست Products هنوز بارگذاری نشده است.
            var products = await _productApiService.GetProductsAsync();

            var product = products.FirstOrDefault(
                p => p.Id == productId);

            if (product == null || !product.IsAvailable)
            {
                return RedirectToPage("/Index");
            }

            _cartService.AddItem(new CartItem
            {
                ProductId = product.Id,
                Name = product.Name,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                Quantity = 1
            });

            return RedirectToPage("/Cart");
        }
    }
}

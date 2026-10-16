using FastFood.Web.Models;
using FastFood.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FastFood.Web.Pages
{
    public class MenuModel : PageModel
    {
        private readonly ProductApiService _productApiService;
        private readonly CategoryApiService _categoryApiService;
        private readonly CartService _cartService;

        public List<ProductDto> Products { get; set; } = new();
        public List<CategoryDto> Categories { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? CategoryId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SortOrder { get; set; }

        public MenuModel(
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
            var products = await _productApiService.GetProductsAsync();

            Categories = await _categoryApiService.GetCategoriesAsync();

            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                products = products
                    .Where(p => !string.IsNullOrWhiteSpace(p.Name) &&
                                p.Name.Contains(
                                    SearchTerm.Trim(),
                                    StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (CategoryId.HasValue)
            {
                products = products
                    .Where(p => p.CategoryId == CategoryId.Value)
                    .ToList();
            }

            products = SortOrder switch
            {
                "price_asc" => products
                    .OrderBy(p => p.Price)
                    .ToList(),

                "price_desc" => products
                    .OrderByDescending(p => p.Price)
                    .ToList(),

                _ => products
            };

            Products = products;
        }

        public async Task<JsonResult> OnGetSuggestionsAsync(string term)
        {
            if (string.IsNullOrWhiteSpace(term) ||
                term.Trim().Length < 3)
            {
                return new JsonResult(new List<string>());
            }

            var products = await _productApiService.GetProductsAsync();

            var suggestions = products
                .Where(p => !string.IsNullOrWhiteSpace(p.Name) &&
                            p.Name.Contains(
                                term.Trim(),
                                StringComparison.OrdinalIgnoreCase))
                .Select(p => p.Name)
                .Distinct()
                .Take(8)
                .ToList();

            return new JsonResult(suggestions);
        }

        public async Task<IActionResult> OnPostAddToCartAsync(int productId)
        {
            var products = await _productApiService.GetProductsAsync();

            var product = products.FirstOrDefault(
                p => p.Id == productId);

            if (product == null || !product.IsAvailable)
            {
                return RedirectToPage("/Menu");
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

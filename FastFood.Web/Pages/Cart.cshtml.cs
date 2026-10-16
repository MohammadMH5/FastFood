using FastFood.Web.Models;
using FastFood.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FastFood.Web.Pages
{
    public class CartModel : PageModel
    {
        private readonly CartService _cartService;

        public List<CartItem> Items { get; private set; } = new();

        public decimal Total =>
            Items.Sum(item => item.Price * item.Quantity);

        public CartModel(CartService cartService)
        {
            _cartService = cartService;
        }

        public void OnGet()
        {
            LoadCart();
        }

        public IActionResult OnPostUpdateQuantity(int productId, int quantity)
        {
            _cartService.UpdateQuantity(productId, quantity);

            return RedirectToPage();
        }

        public IActionResult OnPostRemoveItem(int productId)
        {
            _cartService.RemoveItem(productId);

            return RedirectToPage();
        }

        private void LoadCart()
        {
            Items = _cartService.GetItems();
        }
    }
}
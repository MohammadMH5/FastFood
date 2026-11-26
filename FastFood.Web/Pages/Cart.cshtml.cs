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

        [BindProperty]
        public int? OrderTypeId { get; set; }

        public string? ErrorMessage { get; private set; }

        public decimal Total =>
            Items.Sum(item => item.Price * item.Quantity);

        public CartModel(CartService cartService)
        {
            _cartService = cartService;
        }

        public void OnGet()
        {
            LoadCart();
            OrderTypeId = _cartService.GetOrderTypeId();
        }

        public IActionResult OnPostUpdateQuantity(
            int productId,
            int quantity)
        {
            _cartService.UpdateQuantity(productId, quantity);
            return RedirectToPage();
        }

        public IActionResult OnPostRemoveItem(int productId)
        {
            _cartService.RemoveItem(productId);
            return RedirectToPage();
        }

        public IActionResult OnPostContinueToCheckout()
        {
            LoadCart();

            if (Items.Count == 0)
                return RedirectToPage();

            if (OrderTypeId != 1 && OrderTypeId != 2)
            {
                ErrorMessage =
                    "لطفاً نوع سفارش را انتخاب کنید.";

                return Page();
            }

            _cartService.SetOrderTypeId(OrderTypeId.Value);

            return RedirectToPage("/Checkout");
        }

        private void LoadCart()
        {
            Items = _cartService.GetItems();
        }
    }
}

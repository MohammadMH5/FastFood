using FastFood.Web.Models;
using FastFood.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FastFood.Web.Pages
{
    public class CheckoutModel : PageModel
    {
        private readonly CartService _cartService;
        private readonly CartApiService _cartApiService;
        private readonly OrderApiService _orderApiService;

        public List<CartItem> Items { get; private set; } = new();

        public decimal Total =>
            Items.Sum(item => item.Price * item.Quantity);

        public int? OrderId { get; private set; }

        public string? ErrorMessage { get; private set; }

        public CheckoutModel(
            CartService cartService,
            CartApiService cartApiService,
            OrderApiService orderApiService)
        {
            _cartService = cartService;
            _cartApiService = cartApiService;
            _orderApiService = orderApiService;
        }

        public void OnGet()
        {
            Items = _cartService.GetItems();
        }

        public async Task<IActionResult> OnPostPlaceOrderAsync()
        {
            Items = _cartService.GetItems();

            if (!(User.Identity?.IsAuthenticated ?? false))
            {
                return RedirectToPage("/Login", new
                {
                    ReturnUrl = "/Checkout"
                });
            }

            if (Items.Count == 0)
            {
                ErrorMessage = "سبد خرید شما خالی است.";
                return Page();
            }

            // بررسی وجود سبد خرید در API
            var cartExists = await _cartApiService.CartExistsAsync();

            if (cartExists)
            {
                var cleared = await _cartApiService.ClearCartAsync();

                if (!cleared)
                {
                    ErrorMessage =
                        "پاک‌سازی سبد خرید سرور ناموفق بود.";
                    return Page();
                }
            }

            // انتقال محصولات سبد سایت به API
            foreach (var item in Items)
            {
                var added = await _cartApiService.AddItemAsync(
                    item.ProductId,
                    item.Quantity);

                if (!added)
                {
                    ErrorMessage =
                        "انتقال محصولات به سرور ناموفق بود. سفارش ثبت نشد.";
                    return Page();
                }
            }

            // ثبت سفارش در API
            var order = await _orderApiService.CreateOrderAsync();

            if (order == null)
            {
                ErrorMessage =
                    "پاسخ موفقی از ثبت سفارش دریافت نشد. قبل از تلاش مجدد، سفارش‌های خود را بررسی کنید.";
                return Page();
            }

            // پاک کردن سبد سایت پس از دریافت سفارش موفق
            foreach (var item in Items.ToList())
            {
                _cartService.RemoveItem(item.ProductId);
            }

            OrderId = order.Id;
            Items = new List<CartItem>();

            return Page();
        }
    }
}

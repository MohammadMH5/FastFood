using FastFood.Web.Models;
using FastFood.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FastFood.Web.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class OrdersModel : PageModel
    {
        private readonly OrderApiService _orderApiService;

        public OrdersModel(OrderApiService orderApiService)
        {
            _orderApiService = orderApiService;
        }

        public List<OrderViewModel> Orders { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var orders =
                await _orderApiService.GetAllOrdersForAdminAsync();

            if (orders == null)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "دریافت سفارش‌ها ناموفق بود.");

                return Page();
            }

            Orders = orders;
            return Page();
        }

        public async Task<IActionResult> OnPostUpdateStatusAsync(
            int orderId, string status)
        {
            var allowedStatuses = new[]
            {
                "Pending", "Preparing", "Ready",
                "Completed", "Cancelled"
            };

            if (!allowedStatuses.Contains(status))
            {
                ModelState.AddModelError(
                    string.Empty, "وضعیت انتخاب‌شده معتبر نیست.");

                return await OnGetAsync();
            }

            var updated =
                await _orderApiService.UpdateOrderStatusAsync(
                    orderId, status);

            if (!updated)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "تغییر وضعیت سفارش ناموفق بود.");
            }

            return RedirectToPage();
        }
    }
}

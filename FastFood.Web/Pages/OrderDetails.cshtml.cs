using FastFood.Web.Models;
using FastFood.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FastFood.Web.Pages
{
    [Authorize]
    public class OrderDetailsModel : PageModel
    {
        private readonly OrderApiService _orderApiService;

        public OrderViewModel? Order { get; private set; }

        public string? ErrorMessage { get; private set; }

        public OrderDetailsModel(OrderApiService orderApiService)
        {
            _orderApiService = orderApiService;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id <= 0)
                return NotFound();

            Order = await _orderApiService.GetOrderByIdAsync(id);

            if (Order == null)
            {
                ErrorMessage =
                    "دریافت جزئیات سفارش ناموفق بود. لطفاً دوباره وارد حساب کاربری شوید.";

                return Page();
            }

            return Page();
        }
    }
}

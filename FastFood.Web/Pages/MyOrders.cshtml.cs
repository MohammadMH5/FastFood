using FastFood.Web.Models;
using FastFood.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FastFood.Web.Pages
{
    [Authorize]
    public class MyOrdersModel : PageModel
    {
        private readonly OrderApiService _orderApiService;

        public List<OrderViewModel> Orders { get; private set; } = new();

        public string? ErrorMessage { get; private set; }

        public MyOrdersModel(OrderApiService orderApiService)
        {
            _orderApiService = orderApiService;
        }

        public async Task OnGetAsync()
        {
            var orders = await _orderApiService.GetMyOrdersAsync();

            if (orders == null)
            {
                ErrorMessage =
                    "دریافت سفارش‌ها ناموفق بود. لطفاً دوباره وارد حساب کاربری شوید.";
                return;
            }

            Orders = orders;
        }
    }
}

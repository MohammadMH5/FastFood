using FastFood.Api.DTOs.Orders;
using FastFood.Api.Exceptions;
using FastFood.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FastFood.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        private bool TryGetUserId(out int userId)
        {
            userId = 0;

            var userIdClaim =
                User.FindFirst(ClaimTypes.NameIdentifier);

            return userIdClaim != null
                && int.TryParse(userIdClaim.Value, out userId)
                && userId > 0;
        }

        // Customer: ایجاد سفارش از سبد خرید
        [HttpPost]
        public async Task<IActionResult> CreateOrder(
            [FromBody] CreateOrderDto dto)
        {
            if (!TryGetUserId(out var userId))
            {
                return Unauthorized();
            }

            if (dto.OrderTypeId != 1 && dto.OrderTypeId != 2)
            {
                return BadRequest(new
                {
                    message = "نوع سفارش نامعتبر است."
                });
            }

            var order =
                await _orderService.CreateOrderFromCartAsync(
                    userId,
                    dto.OrderTypeId);

            if (order == null)
            {
                return BadRequest(new
                {
                    message = "سبد خرید خالی است."
                });
            }

            return Ok(order);
        }

        // Customer: مشاهده سفارش‌های خود
        [HttpGet]
        public async Task<IActionResult> GetMyOrders()
        {
            if (!TryGetUserId(out var userId))
            {
                return Unauthorized();
            }

            var orders =
                await _orderService.GetMyOrdersAsync(userId);

            return Ok(orders);
        }

        // Customer: مشاهده یک سفارش از سفارش‌های خود
        [HttpGet("{orderId:int}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            if (!TryGetUserId(out var userId))
            {
                return Unauthorized();
            }

            if (orderId <= 0)
            {
                return BadRequest(new
                {
                    message = "شناسه سفارش نامعتبر است."
                });
            }

            var order =
                await _orderService.GetOrderByIdAsync(
                    orderId,
                    userId);

            if (order == null)
            {
                return NotFound(new
                {
                    message = "سفارش مورد نظر پیدا نشد."
                });
            }

            return Ok(order);
        }

        // Admin: مشاهده تمام سفارش‌ها
        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllOrdersForAdmin()
        {
            var orders = await _orderService.GetAllOrdersAsync();

            return Ok(orders);
        }

        // Admin: مشاهده یک سفارش
        [HttpGet("admin/{orderId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetOrderByIdForAdmin(
            int orderId)
        {
            if (orderId <= 0)
            {
                return BadRequest(new
                {
                    message = "شناسه سفارش نامعتبر است."
                });
            }

            var order =
                await _orderService.GetOrderByIdForAdminAsync(
                    orderId);

            if (order == null)
            {
                return NotFound(new
                {
                    message = "سفارش مورد نظر پیدا نشد."
                });
            }

            return Ok(order);
        }

        // Admin: تغییر وضعیت سفارش
        [HttpPut("admin/{orderId:int}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateOrderStatus(
            int orderId,
            [FromBody] UpdateOrderStatusDto dto)
        {
            if (orderId <= 0)
            {
                return BadRequest(new
                {
                    message = "شناسه سفارش نامعتبر است."
                });
            }

            if (dto == null ||
                string.IsNullOrWhiteSpace(dto.Status))
            {
                return BadRequest(new
                {
                    message = "وضعیت سفارش الزامی است."
                });
            }

            var result =
                await _orderService.UpdateOrderStatusAsync(
                    orderId,
                    dto.Status);

            if (!result)
            {
                return NotFound(new
                {
                    message = "سفارش مورد نظر پیدا نشد."
                });
            }

            return Ok(new
            {
                message = "وضعیت سفارش با موفقیت تغییر کرد."
            });
        }
    }
}

using FastFood.Api.DTOs.Carts;
using FastFood.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FastFood.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
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

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            if (!TryGetUserId(out var userId))
            {
                return Unauthorized();
            }

            var cart = await _cartService.GetCartAsync(userId);

            if (cart == null)
            {
                return NotFound("سبد خرید پیدا نشد.");
            }

            return Ok(cart);
        }

        [HttpPost("items")]
        public async Task<IActionResult> AddItem(
            [FromBody] AddCartItemDto dto)
        {
            if (!TryGetUserId(out var userId))
            {
                return Unauthorized();
            }

            if (dto == null || dto.ProductId <= 0)
            {
                return BadRequest("شناسه محصول نامعتبر است.");
            }

            try
            {
                await _cartService.AddItemAsync(
                    userId,
                    dto.ProductId,
                    dto.Quantity);

                return Ok("محصول با موفقیت به سبد خرید اضافه شد.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("items/{productId:int}")]
        public async Task<IActionResult> UpdateItem(
            int productId,
            [FromBody] UpdateCartItemDto dto)
        {
            if (!TryGetUserId(out var userId))
            {
                return Unauthorized();
            }

            if (productId <= 0)
            {
                return BadRequest("شناسه محصول نامعتبر است.");
            }

            if (dto == null)
            {
                return BadRequest("اطلاعات تعداد محصول الزامی است.");
            }

            try
            {
                var result = await _cartService.UpdateItemAsync(
                    userId,
                    productId,
                    dto.Quantity);

                if (!result)
                {
                    return NotFound(
                        "محصول مورد نظر در سبد خرید پیدا نشد.");
                }

                return Ok("تعداد محصول با موفقیت تغییر کرد.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("items/{productId:int}")]
        public async Task<IActionResult> RemoveItem(int productId)
        {
            if (!TryGetUserId(out var userId))
            {
                return Unauthorized();
            }

            if (productId <= 0)
            {
                return BadRequest("شناسه محصول نامعتبر است.");
            }

            var result = await _cartService.RemoveItemAsync(
                userId,
                productId);

            if (!result)
            {
                return NotFound(
                    "محصول مورد نظر در سبد خرید پیدا نشد.");
            }

            return Ok("محصول با موفقیت از سبد خرید حذف شد.");
        }

        [HttpDelete]
        public async Task<IActionResult> ClearCart()
        {
            if (!TryGetUserId(out var userId))
            {
                return Unauthorized();
            }

            var result = await _cartService.ClearCartAsync(userId);

            if (!result)
            {
                return NotFound("سبد خرید پیدا نشد.");
            }

            return Ok("سبد خرید با موفقیت خالی شد.");
        }
    }
}

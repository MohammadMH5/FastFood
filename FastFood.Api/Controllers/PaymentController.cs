using FastFood.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FastFood.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(
            IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("{orderId}")]
        public async Task<IActionResult> CreatePayment(
            int orderId)
        {
            var userId = int.Parse(
                User.FindFirst(
                    ClaimTypes.NameIdentifier)!.Value);

            var payment =
                await _paymentService
                    .CreatePaymentAsync(
                        orderId,
                        userId);

            if (payment == null)
            {
                return NotFound(
                    "سفارش مورد نظر پیدا نشد.");
            }

            return Ok(payment);
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetPayment(
            int orderId)
        {
            var userId = int.Parse(
                User.FindFirst(
                    ClaimTypes.NameIdentifier)!.Value);

            var payment =
                await _paymentService
                    .GetPaymentByOrderIdAsync(
                        orderId,
                        userId);

            if (payment == null)
            {
                return NotFound(
                    "اطلاعات پرداخت مورد نظر پیدا نشد.");
            }

            return Ok(payment);
        }
    }
}
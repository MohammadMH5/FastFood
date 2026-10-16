using FastFood.Api.DTOs.Auth;
using FastFood.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FastFood.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginRequestDto request)
        {
            var result = await _authService.LoginAsync(
                request.Username,
                request.Password);

            if (result == null)
            {
                return Unauthorized(new
                {
                    message = "نام کاربری یا رمز عبور اشتباه است."
                });
            }

            return Ok(result);
        }
    }
}

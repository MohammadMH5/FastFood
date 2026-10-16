using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using FastFood.Web.Models;
using FastFood.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FastFood.Web.Pages
{
    public class LoginModel : PageModel
    {
        private readonly AuthApiService _authApiService;

        public LoginModel(AuthApiService authApiService)
        {
            _authApiService = authApiService;
        }

        [BindProperty]
        public LoginInput Input { get; set; } = new();

        public string? ErrorMessage { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? ReturnUrl { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var request = new LoginRequest
            {
                Username = Input.Username,
                Password = Input.Password
            };

            var result = await _authApiService.LoginAsync(request);

            if (result == null ||
                string.IsNullOrWhiteSpace(result.Token) ||
                string.IsNullOrWhiteSpace(result.Role))
            {
                ErrorMessage =
                    "ورود ناموفق بود یا اطلاعات نقش کاربر دریافت نشد.";

                return Page();
            }

            HttpContext.Session.SetString(
                "AccessToken",
                result.Token);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, Input.Username),
                new Claim(ClaimTypes.Role, result.Role)
            };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = false,
                AllowRefresh = false,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                authProperties);

            if (!string.IsNullOrWhiteSpace(ReturnUrl) &&
                Url.IsLocalUrl(ReturnUrl))
            {
                return LocalRedirect(ReturnUrl);
            }

            return RedirectToPage("/Index");
        }

        public class LoginInput
        {
            [Required(ErrorMessage = "نام کاربری را وارد کنید.")]
            public string Username { get; set; } = string.Empty;

            [Required(ErrorMessage = "رمز عبور را وارد کنید.")]
            public string Password { get; set; } = string.Empty;
        }
    }
}

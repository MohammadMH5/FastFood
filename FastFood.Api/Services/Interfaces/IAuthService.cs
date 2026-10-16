using FastFood.Api.DTOs.Auth;

namespace FastFood.Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> LoginAsync(string username, string password);
    }
}

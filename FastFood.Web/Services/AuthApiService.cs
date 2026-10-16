using System.Net.Http.Json;
using FastFood.Web.Models;

namespace FastFood.Web.Services
{
    public class AuthApiService
    {
        private readonly HttpClient _httpClient;

        public AuthApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<LoginResponse?> LoginAsync(
            LoginRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync(
                "api/Auth/login",
                request);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content
                .ReadFromJsonAsync<LoginResponse>();
        }

    }
}

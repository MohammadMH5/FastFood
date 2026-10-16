
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FastFood.Web.Models;

namespace FastFood.Web.Services
{
    public class CartApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartApiService(
            HttpClient httpClient,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        private HttpRequestMessage? CreateRequest(
            HttpMethod method,
            string url)
        {
            var token = _httpContextAccessor.HttpContext?
                .Session.GetString("AccessToken");

            if (string.IsNullOrWhiteSpace(token))
                return null;

            var request = new HttpRequestMessage(method, url);
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            return request;
        }

        public async Task<bool> CartExistsAsync()
        {
            using var request = CreateRequest(
                HttpMethod.Get, "api/Cart");

            if (request == null)
                return false;

            using var response =
                await _httpClient.SendAsync(request);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AddItemAsync(
            int productId,
            int quantity)
        {
            using var request = CreateRequest(
                HttpMethod.Post, "api/Cart/items");

            if (request == null)
                return false;

            request.Content = JsonContent.Create(new
            {
                ProductId = productId,
                Quantity = quantity
            });

            using var response =
                await _httpClient.SendAsync(request);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveItemAsync(int productId)
        {
            using var request = CreateRequest(
                HttpMethod.Delete,
                $"api/Cart/items/{productId}");

            if (request == null)
                return false;

            using var response =
                await _httpClient.SendAsync(request);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ClearCartAsync()
        {
            using var request = CreateRequest(
                HttpMethod.Delete, "api/Cart");

            if (request == null)
                return false;

            using var response =
                await _httpClient.SendAsync(request);

            return response.IsSuccessStatusCode;
        }
    }
}

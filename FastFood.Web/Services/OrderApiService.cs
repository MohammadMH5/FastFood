using System.Net.Http.Headers;
using System.Net.Http.Json;
using FastFood.Web.Models;

namespace FastFood.Web.Services
{
    public class OrderApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderApiService(
            HttpClient httpClient,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<OrderResponse?> CreateOrderAsync(int orderTypeId)
        {
            if (orderTypeId != 1 && orderTypeId != 2)
                return null;

            var token = _httpContextAccessor.HttpContext?
                .Session.GetString("AccessToken");

            if (string.IsNullOrWhiteSpace(token))
                return null;

            using var request = new HttpRequestMessage(
                HttpMethod.Post, "api/Order");

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            request.Content = JsonContent.Create(new
            {
                OrderTypeId = orderTypeId
            });

            using var response =
                await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content
                .ReadFromJsonAsync<OrderResponse>();
        }

        public async Task<OrderViewModel?> GetOrderByIdAsync(int orderId)
        {
            var token = _httpContextAccessor.HttpContext?
                .Session.GetString("AccessToken");

            if (string.IsNullOrWhiteSpace(token))
                return null;

            using var request = new HttpRequestMessage(
                HttpMethod.Get, $"api/Order/{orderId}");

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            using var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content
                .ReadFromJsonAsync<OrderViewModel>();
        }

        public async Task<List<OrderViewModel>?> GetMyOrdersAsync()
        {
            var token = _httpContextAccessor.HttpContext?
                .Session.GetString("AccessToken");

            if (string.IsNullOrWhiteSpace(token))
                return null;

            using var request = new HttpRequestMessage(
                HttpMethod.Get, "api/Order");

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            using var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content
                .ReadFromJsonAsync<List<OrderViewModel>>();
        }

        public async Task<List<OrderViewModel>?> GetAllOrdersForAdminAsync()
        {
            var token = _httpContextAccessor.HttpContext?
                .Session.GetString("AccessToken");

            if (string.IsNullOrWhiteSpace(token))
                return null;

            using var request = new HttpRequestMessage(
                HttpMethod.Get, "api/Order/admin");

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            using var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content
                .ReadFromJsonAsync<List<OrderViewModel>>();
        }

        public async Task<bool> UpdateOrderStatusAsync(
            int orderId, string status)
        {
            var token = _httpContextAccessor.HttpContext?
                .Session.GetString("AccessToken");

            if (string.IsNullOrWhiteSpace(token))
                return false;

            using var request = new HttpRequestMessage(
                HttpMethod.Put,
                $"api/Order/admin/{orderId}/status");

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            request.Content = JsonContent.Create(new
            {
                Status = status
            });

            using var response = await _httpClient.SendAsync(request);

            return response.IsSuccessStatusCode;
        }
    }

    public class OrderResponse
    {
        public int Id { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}

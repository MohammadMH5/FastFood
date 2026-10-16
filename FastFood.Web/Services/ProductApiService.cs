using System.Net.Http.Json;
using FastFood.Web.Models;

namespace FastFood.Web.Services
{
    public class ProductApiService
    {
        private readonly HttpClient _httpClient;

        public ProductApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ProductDto>> GetProductsAsync()
        {
            var products =
                await _httpClient
                    .GetFromJsonAsync<List<ProductDto>>(
                        "api/Products");

            return products ?? new List<ProductDto>();
        }
    }
}
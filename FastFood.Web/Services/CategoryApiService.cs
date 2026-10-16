using System.Net.Http.Json;
using FastFood.Web.Models;

namespace FastFood.Web.Services
{
    public class CategoryApiService
    {
        private readonly HttpClient _httpClient;

        public CategoryApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CategoryDto>> GetCategoriesAsync()
        {
            var categories =
                await _httpClient
                    .GetFromJsonAsync<List<CategoryDto>>(
                        "api/Categories");

            return categories ?? new List<CategoryDto>();
        }
    }
}
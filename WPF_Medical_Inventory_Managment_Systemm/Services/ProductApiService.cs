using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using WPF_Medical_Inventory_Managment_Systemm.Models;

namespace WPF_Medical_Inventory_Managment_Systemm.Services
{
    public class ProductApiService
    {
        private readonly HttpClient _client;

        public ProductApiService()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7228/api/")  // https://localhost:7228;http://localhost:5000
            };
        }
        //public ProductApiService(HttpClient httpClient)
        //{
        //    _client = httpClient;
        //    _client.BaseAddress = new Uri("https://localhost:7228/api/");
        //}
        public ProductApiService(HttpClient httpClient)
        {
            _client = httpClient;
        }



        // Get All Products
        public async Task<List<Product>> GetAllProductsAsync()
        {
            //var response = await _client.GetAsync("api/Products");
            var response = await _client.GetAsync("Products");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Product>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            return new List<Product>();
        }

        // Create Product
        public async Task<bool> CreateProductAsync(CreateProductDTO product)
        {
            var response = await _client.PostAsJsonAsync("Products", product);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateProductAsync(int id, UpdateProductDTO product)
        {
            var response = await _client.PutAsJsonAsync($"Products/{id}", product);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var response = await _client.DeleteAsync($"Products/{id}");
            return response.IsSuccessStatusCode;
        }
        // Inside ProductApiService class
        public async Task<Product> GetProductByIdAsync(int id)
        {
            var response = await _client.GetAsync($"Products/{id}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Product>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            return null;
        }


    }
}

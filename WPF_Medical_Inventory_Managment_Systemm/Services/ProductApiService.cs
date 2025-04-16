using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
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
    }
}

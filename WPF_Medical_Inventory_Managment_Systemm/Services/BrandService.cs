using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WPF_Medical_Inventory_Managment_Systemm.Models;

namespace WPF_Medical_Inventory_Managment_Systemm.Services
{
    public class BrandService
    {
        private readonly HttpClient _httpClient;

        public BrandService(HttpClient httpClient)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7228/api/")
            };
        }

        public async Task<List<Brand>> GetBrandsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Brand>>("brands");
        }

        public async Task AddBrandAsync(Brand brand)
        {
            var response = await _httpClient.PostAsJsonAsync("brands", brand);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateBrandAsync(Brand brand)
        {
            var response = await _httpClient.PutAsJsonAsync($"brands/{brand.Id}", brand);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteBrandAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"brands/{id}");
            response.EnsureSuccessStatusCode();
        }
        public async Task<Brand> GetBrandByIdAsync(int brandId)
        {
            return await _httpClient.GetFromJsonAsync<Brand>($"brands/{brandId}");
        }

    }
}

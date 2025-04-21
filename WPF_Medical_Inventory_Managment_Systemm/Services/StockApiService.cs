using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WPF_Medical_Inventory_Managment_Systemm.Models;

namespace WPF_Medical_Inventory_Managment_Systemm.Services
{
    public class StockApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "http://localhost:5000/api/stock"; // Replace with your actual backend URL

        public StockApiService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<StockDto>> GetAllStockAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<StockDto>>(_baseUrl);

        }

        public async Task<List<StockDto>> GetLowStockAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<StockDto>>($"{_baseUrl}/low-stock");

        }

        public async Task<List<StockDto>> GetNearExpiryStockAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<StockDto>>($"{_baseUrl}/near-expiry");

        }

        public async Task UpdateStockAfterSaleAsync(int productId, int quantitySold)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/update-stock-sale", new { productId, quantitySold });
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateStockAfterPurchaseAsync(int productId, int quantityPurchased)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/update-stock-purchase", new { productId, quantityPurchased });
            response.EnsureSuccessStatusCode();
        }

        public async Task AddStockToProductAsync(int productId, int quantityToAdd)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/add-stock", new { productId, quantityToAdd });
            response.EnsureSuccessStatusCode();
        }
    }
}

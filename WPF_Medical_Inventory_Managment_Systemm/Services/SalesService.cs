using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using WPF_Medical_Inventory_Managment_Systemm.Models;

namespace WPF_Medical_Inventory_Managment_Systemm.Services
{
    public class SalesService
    {
        private readonly HttpClient _httpClient;

        public SalesService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<SaleResponseDto> CreateSaleAsync(List<SaleItemDto> saleItems, string customerName)
        {
            var saleRequest = new
            {
                CustomerName = customerName,
                SaleDate = DateTime.Now,
                Items = saleItems
            };

            var response = await _httpClient.PostAsJsonAsync("https://localhost:7228/api/Sales", saleRequest);

            if (response.IsSuccessStatusCode)
            {
                var saleResponse = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<SaleResponseDto>(saleResponse);
            }

            return null;
        }
    }
}

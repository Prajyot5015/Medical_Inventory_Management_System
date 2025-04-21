using System.Net.Http;
using System.Net.Http.Json;
using WPF_Medical_Inventory_Managment_Systemm.Models;

namespace WPF_Medical_Inventory_Managment_Systemm.Services
{
    public class SalesApiService
    {
        private readonly HttpClient _httpClient;

        public SalesApiService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7228/api/")
            };
        }

        public async Task<SaleResponseDto> CreateSaleAsync(CreateSaleDto sale)
        {
            var response = await _httpClient.PostAsJsonAsync("Sales", sale);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<SaleResponseDto>();
            }
            return null;
        }
    }

}

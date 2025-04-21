using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
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

        public async Task<byte[]> GetInvoiceAsync(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsByteArrayAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching invoice: {ex.Message}");
                return null;
            }
        }


        public async Task<List<SaleResponseDto>> GetAllSalesAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<SaleResponseDto>>("Sales");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching sales: {ex.Message}");
                return new List<SaleResponseDto>();
            }
        }

        public async Task<SaleResponseDto> GetSaleByIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<SaleResponseDto>($"Sales/{id}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching sale by ID: {ex.Message}");
                return null;
            }
        }


    }

}

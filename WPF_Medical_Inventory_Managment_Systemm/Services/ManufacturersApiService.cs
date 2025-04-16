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
    public class ManufacturersApiService
    {
        private readonly HttpClient _httpClient;

        public ManufacturersApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7228/api/");
        }

        public async Task<List<ManufacturersDTO>> GetAllManufacturersAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ManufacturersDTO>>("Manufacturers");
        }

        public async Task<ManufacturersDTO> GetManufacturerByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<ManufacturersDTO>($"Manufacturers/{id}");
        }

        public async Task<string> AddManufacturerAsync(ManufacturersDTO dto)
        {
            var response = await _httpClient.PostAsJsonAsync("Manufacturers", dto);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> UpdateManufacturerAsync(int id, ManufacturersDTO dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"Manufacturers/{id}", dto);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> DeleteManufacturerAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"Manufacturers/{id}");
            return await response.Content.ReadAsStringAsync();
        }
    }
}

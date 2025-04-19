using System.Net.Http;
using System.Net.Http.Json;
using WPF_Medical_Inventory_Managment_Systemm.Models;

namespace WPF_Medical_Inventory_Managment_Systemm.Services
{
    public class ManufacturerService
    {
        private readonly HttpClient _httpClient;

        public ManufacturerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Manufacturer>> GetManufacturerAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Manufacturer>>("manufacturers");
        }

        public async Task AddManufacturerAsync(Manufacturer manufacturer)
        {
            var response = await _httpClient.PostAsJsonAsync("manufacturers", manufacturer);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateManufacturerAsync(Manufacturer manufacturer)
        {
            var response = await _httpClient.PutAsJsonAsync($"manufacturers/{manufacturer.Id}", manufacturer);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteManufacturerAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"manufacturers/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}

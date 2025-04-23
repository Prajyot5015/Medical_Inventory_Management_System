using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WPF_Medical_Inventory_Managment_Systemm.Models;

namespace WPF_Medical_Inventory_Managment_Systemm.Services
{
    public class PurchaseOrderApiService
    {
        private readonly HttpClient _httpClient;

        public PurchaseOrderApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            //_httpClient.BaseAddress = new Uri("https://localhost:7228/api/");
        }

        
        public async Task<List<PurchaseOrderDTO>> GetAllPurchaseOrdersAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<PurchaseOrderDTO>>("PurchaseOrders");
            }
            catch (Exception ex)
            {
            
                throw new ApplicationException("Error retrieving purchase orders.", ex);
            }
        }

      
        public async Task<PurchaseOrderDTO> GetPurchaseOrderByIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<PurchaseOrderDTO>($"PurchaseOrders/{id}");
            }
            catch (Exception ex)
            {
           
                throw new ApplicationException($"Error retrieving purchase order with ID {id}.", ex);
            }
        }

        public async Task<string> AddPurchaseOrderAsync(PurchaseOrderDTO dto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("PurchaseOrders", dto);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                   
                    throw new ApplicationException($"Failed to add purchase order: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
     
                throw new ApplicationException("Error adding purchase order.", ex);
            }
        }

        // Uncomment if Update is needed in the future
        //public async Task<string> UpdatePurchaseOrderAsync(PurchaseOrderDTO dto)
        //{
        //    var response = await _httpClient.PutAsJsonAsync($"PurchaseOrders/{dto.Id}", dto);
        //    return await response.Content.ReadAsStringAsync();
        //}

        // Uncomment if Delete is needed in the future
        //public async Task<string> DeletePurchaseOrderAsync(int id)
        //{
        //    var response = await _httpClient.DeleteAsync($"PurchaseOrders/{id}");
        //    return await response.Content.ReadAsStringAsync();
        //}
    }
}




//using System;
//using System.Collections.Generic;
//using System.Net.Http;
//using System.Net.Http.Json;
//using System.Threading.Tasks;
//using WPF_Medical_Inventory_Managment_Systemm.Models;

//namespace WPF_Medical_Inventory_Managment_Systemm.Services
//{
//    public class PurchaseOrderApiService
//    {
//        private readonly HttpClient _httpClient;

//        public PurchaseOrderApiService(HttpClient httpClient)
//        {
//            _httpClient = httpClient;
//            _httpClient.BaseAddress = new Uri("https://localhost:7228/api/");
//        }

//        public async Task<List<PurchaseOrderDTO>> GetAllPurchaseOrdersAsync()
//        {
//            return await _httpClient.GetFromJsonAsync<List<PurchaseOrderDTO>>("PurchaseOrders");
//        }

//        public async Task<PurchaseOrderDTO> GetPurchaseOrderByIdAsync(int id)
//        {
//            return await _httpClient.GetFromJsonAsync<PurchaseOrderDTO>($"PurchaseOrders/{id}");
//        }

//        public async Task<string> AddPurchaseOrderAsync(PurchaseOrderDTO dto)
//        {
//            var response = await _httpClient.PostAsJsonAsync("PurchaseOrders", dto);
//            return await response.Content.ReadAsStringAsync();
//        }

//public async Task<string> UpdatePurchaseOrderAsync(PurchaseOrderDTO dto)
//{
//    var response = await _httpClient.PutAsJsonAsync($"PurchaseOrders/{dto.Id}", dto);
//    return await response.Content.ReadAsStringAsync();
//}

//public async Task<string> DeletePurchaseOrderAsync(int id)
//{
//    var response = await _httpClient.DeleteAsync($"PurchaseOrders/{id}");
//    return await response.Content.ReadAsStringAsync();
//}
//    }
//}



//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Net.Http;
//using System.Net.Http.Json;
//using System.Text;
//using System.Threading.Tasks;
//using WPF_Medical_Inventory_Managment_Systemm.Models;

//namespace WPF_Medical_Inventory_Managment_Systemm.Services
//{
//    public class PurchaseOrderApiService
//    {
//        private readonly HttpClient _httpClient;

//        public PurchaseOrderApiService(HttpClient httpClient)
//        {
//            _httpClient = httpClient;
//            _httpClient.BaseAddress = new Uri("https://localhost:7228/api/");
//        }

//        public async Task<List<PurchaseOrderDTO>> GetAllPurchaseOrdersAsync()
//        {
//            return await _httpClient.GetFromJsonAsync<List<PurchaseOrderDTO>>("PurchaseOrders");
//        }

//        public async Task<PurchaseOrderDTO> GetPurchaseOrderByIdAsync(int id)
//        {
//            return await _httpClient.GetFromJsonAsync<PurchaseOrderDTO>($"PurchaseOrders/{id}");
//        }

//        public async Task<string> AddPurchaseOrderAsync(PurchaseOrderDTO dto)
//        {
//            var response = await _httpClient.PostAsJsonAsync("PurchaseOrders", dto);
//            return await response.Content.ReadAsStringAsync();
//        }

//        public async Task<string> UpdatePurchaseOrderAsync(PurchaseOrderDTO dto)
//        {
//            var response = await _httpClient.PutAsJsonAsync($"PurchaseOrders/{dto.Id}", dto);
//            return await response.Content.ReadAsStringAsync();
//        }

//        public async Task<string> DeletePurchaseOrderAsync(int id)
//        {
//            var response = await _httpClient.DeleteAsync($"PurchaseOrders/{id}");
//            return await response.Content.ReadAsStringAsync();
//        }
//    }
//}









//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Net.Http.Json;
//using System.Text;
//using System.Threading.Tasks;
//using WPF_Medical_Inventory_Managment_Systemm.Models;

//namespace WPF_Medical_Inventory_Managment_Systemm.Services
//{
//    public class PurchaseOrderService
//    {
//        private readonly HttpClient _httpClient;

//        public PurchaseOrderService()
//        {
//            _httpClient = new HttpClient
//            {
//                BaseAddress = new Uri("https://localhost:7107/api/") // Use your API base address
//            };
//        }

//        public async Task<List<PurchaseOrder>> GetAllAsync()
//        {
//            return await _httpClient.GetFromJsonAsync<List<PurchaseOrder>>("PurchaseOrder");
//        }

//        public async Task<PurchaseOrder> GetByIdAsync(int id)
//        {
//            return await _httpClient.GetFromJsonAsync<PurchaseOrder>($"PurchaseOrder/{id}");
//        }

//        public async Task<bool> CreateAsync(PurchaseOrder order)
//        {
//            var response = await _httpClient.PostAsJsonAsync("PurchaseOrder", order);
//            return response.IsSuccessStatusCode;
//        }

//        public async Task<bool> UpdateAsync(PurchaseOrder order)
//        {
//            var response = await _httpClient.PutAsJsonAsync($"PurchaseOrder/{order.Id}", order);
//            return response.IsSuccessStatusCode;
//        }

//        public async Task<bool> DeleteAsync(int id)
//        {
//            var response = await _httpClient.DeleteAsync($"PurchaseOrder/{id}");
//            return response.IsSuccessStatusCode;
//        }
//    }
//}

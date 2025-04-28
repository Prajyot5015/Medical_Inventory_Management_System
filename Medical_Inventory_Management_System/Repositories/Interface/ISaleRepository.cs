using Medical_Inventory_Management_System.Models.Domain;
using Medical_Inventory_Management_System.Models.DTOs;

namespace Medical_Inventory_Management_System.Repositories.Interface
{
    public interface ISaleRepository
    {
        Task<Sale> AddSaleAsync(Sale sale);
        Task<List<Sale>> GetAllSalesAsync();
        Task<Product> GetProductByIdAsync(int productId);
        Task UpdateProductAsync(Product product);

        Task<Sale> GetSaleByIdAsync(int id);

        Task<IEnumerable<SaleResponseDto>> SearchSalesAsync(string query);
    }
}

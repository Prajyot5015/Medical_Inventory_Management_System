using Medical_Inventory_Management_System.Models.Domain;

namespace Medical_Inventory_Management_System.Repositories.Interface
{
    public interface ISaleRepository
    {
        Task<Sale> AddSaleAsync(Sale sale);
        Task<List<Sale>> GetAllSalesAsync();
        Task<Product> GetProductByIdAsync(int productId);
        Task UpdateProductAsync(Product product);

        Task<Sale> GetSaleByIdAsync(int id);

    }
}

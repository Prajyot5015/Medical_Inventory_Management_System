using Medical_Inventory_Management_System.Models.Domain;

namespace Medical_Inventory_Management_System.Repositories.Interface
{
    public interface IProductsRepository
    {
        Task<bool> AddProductAsync(Product product);

        Task<List<Product>> GetAllProductsAsync();

        Task<Product> GetProductByIdAsync(int id);

        Task<bool> UpdateProductAsync(int id, Product product);

        Task<bool> DeleteProductAsync(int id);
    }
}

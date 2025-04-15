using Medical_Inventory_Management_System.Models.DTOs;

namespace Medical_Inventory_Management_System.Services.Interface
{
    public interface IProductsService
    {
        Task<string> AddProductAsync(CreateProductDTO createProductDTO);

        Task<List<ProductDTO>> GetProductsAsync();

        Task<ProductDTO> GetProductById(int id);

        Task<string> UpdateProductAsync(int id, UpdateProductDTO updateProductDTO);
    }
}

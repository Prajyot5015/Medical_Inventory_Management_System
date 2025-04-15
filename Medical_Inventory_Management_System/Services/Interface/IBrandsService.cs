using Medical_Inventory_Management_System.Models.Domain;
using Medical_Inventory_Management_System.Models.DTOs;

namespace Medical_Inventory_Management_System.Services.Interface
{
    public interface IBrandsService
    {
        Task<string> AddBrandAsync(CreateBrandDTO brandDto);

        Task<List<BrandDTO>> GetAllBrandsAsync();

        Task<BrandDTO> GetBrandByIdAsync(int id);

        Task<string> UpdateBrandAsync(int id,UpdateBrandDTO brandDto);

        Task<string> DeleteBrandAsync(int id);

    }
}

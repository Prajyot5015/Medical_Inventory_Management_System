using Medical_Inventory_Management_System.Models.Domain;

namespace Medical_Inventory_Management_System.Repositories.Interface
{
    public interface IBrandsRepository
    {
        Task<bool> AddBrandAsync(Brand brand);

        Task<List<Brand>> GetAllBrandsAsync();

        Task<Brand> GetBrandByIdAsync(int id);

        Task<bool> UpdateBrandAsync(int id,Brand brand);

        Task<bool> DeleteBrandAsync(int id);
    }
}

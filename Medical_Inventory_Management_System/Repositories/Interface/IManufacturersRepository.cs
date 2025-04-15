using Medical_Inventory_Management_System.Models.Domain;

namespace Medical_Inventory_Management_System.Repositories.Interface
{
    public interface IManufacturersRepository
    {
        Task<bool> AddManufacturersAsync(Manufacturer manufacturer);

        Task<List<Manufacturer>> GetAllManufacturersAsync();

        Task<Manufacturer> GetManuFacturerById(int id);

        Task<bool> UpdateManufacturerAsync(int id, Manufacturer manufacturer);

        Task<bool> DeleteManufacturer(int id);
    }
}

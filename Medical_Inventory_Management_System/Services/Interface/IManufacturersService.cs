using Medical_Inventory_Management_System.Models.Domain;
using Medical_Inventory_Management_System.Models.DTOs;

namespace Medical_Inventory_Management_System.Services.Interface
{
    public interface IManufacturersService
    {
        Task<string> AddManufacturersAsync(CreateManufacturersDTO createManufacturersDTO);

        Task<List<ManufacturersDTO>> GetAllManuFactures();

        Task<ManufacturersDTO> GetManufacturerById(int id);

        Task<string> UpdateManufacturer(int id, UpdateManuFacturerDTO updateManuFacturerDTO);

        Task<bool> DeleteManufacturer(int id);
    }
}

using AutoMapper;
using Medical_Inventory_Management_System.Models.Domain;
using Medical_Inventory_Management_System.Models.DTOs;
using Medical_Inventory_Management_System.Repositories.Interface;
using Medical_Inventory_Management_System.Services.Interface;

namespace Medical_Inventory_Management_System.Services.Implementations
{
    public class ManufacturersService : IManufacturersService
    {
        private readonly IManufacturersRepository manufacturersRepository;
        private readonly IMapper mapper;

        public ManufacturersService(IManufacturersRepository manufacturersRepository, IMapper mapper)
        {
            this.manufacturersRepository = manufacturersRepository;
            this.mapper = mapper;
        }

        public async Task<string> AddManufacturersAsync(CreateManufacturersDTO createManufacturersDTO)
        {
            var response = await manufacturersRepository.AddManufacturersAsync(mapper.Map<Manufacturer>(createManufacturersDTO));

            return response ? "Manufacture Added Successfully" : "Failed to Add Manufacture";
        }

        public async Task<bool> DeleteManufacturer(int id)
        {
            var response = await manufacturersRepository.DeleteManufacturer(id);

            return response;

        }

        public async Task<List<ManufacturersDTO>> GetAllManuFactures()
        {
            var manufacturers = await manufacturersRepository.GetAllManufacturersAsync();

            return mapper.Map<List<ManufacturersDTO>>(manufacturers);
        }

        public async Task<ManufacturersDTO> GetManufacturerById(int id)
        {
            var manufacturer = await manufacturersRepository.GetManuFacturerById(id);
            if (manufacturer == null) return null;
            return mapper.Map<ManufacturersDTO>(manufacturer);
        }

        public async Task<string> UpdateManufacturer(int id, UpdateManuFacturerDTO updateManuFacturerDTO)
        {
            var response = await manufacturersRepository.UpdateManufacturerAsync(id, mapper.Map<Manufacturer>(updateManuFacturerDTO));

            return response ? "Manufacturer Updated Successfully" : "No Manufacturer Found";
        }
    }
}

using AutoMapper;
using Medical_Inventory_Management_System.Models.Domain;
using Medical_Inventory_Management_System.Models.DTOs;
using Medical_Inventory_Management_System.Repositories.Interface;
using Medical_Inventory_Management_System.Services.Interface;

namespace Medical_Inventory_Management_System.Services.Implementations
{
    public class BrandsService : IBrandsService
    {
        private readonly IBrandsRepository brandsRepository;
        private readonly IMapper mapper;

        public BrandsService(IBrandsRepository brandsRepository, IMapper mapper)
        {
            this.brandsRepository = brandsRepository;
            this.mapper = mapper;
        }

        public async Task<string> AddBrandAsync(CreateBrandDTO brandDto)
        {
            var brandDomain = mapper.Map<Brand>(brandDto);

            var response = await brandsRepository.AddBrandAsync(brandDomain);

            return response ? "Brand added successfully" : "Failed to add brand";
        }

        public async Task<string> DeleteBrandAsync(int id)
        {
            var response = await brandsRepository.DeleteBrandAsync(id);
            
            return response ? "Brand deleted successfully" : "No Brand Found";
        }

        public async Task<List<BrandDTO>> GetAllBrandsAsync()
        {
            var brands = await brandsRepository.GetAllBrandsAsync();

            return mapper.Map<List<BrandDTO>>(brands);
        }

        public async Task<BrandDTO> GetBrandByIdAsync(int id)
        {
            var brand = await brandsRepository.GetBrandByIdAsync(id);
            if (brand == null) return null;

            return mapper.Map<BrandDTO>(brand);
        }

        public async Task<string> UpdateBrandAsync(int id, UpdateBrandDTO brandDto)
        {
            var brandDomain = mapper.Map<Brand>(brandDto);
            var response = await brandsRepository.UpdateBrandAsync(id,brandDomain);
            return response ? "Brand updated successfully" : "No Brand Found";
        }
    }
}

using AutoMapper;
using Medical_Inventory_Management_System.Models.Domain;
using Medical_Inventory_Management_System.Models.DTOs;
using Medical_Inventory_Management_System.Repositories.Interface;
using Medical_Inventory_Management_System.Services.Interface;

namespace Medical_Inventory_Management_System.Services.Implementations
{
    public class ProductsService : IProductsService
    {
        private readonly IProductsRepository productsRepository;
        private readonly IMapper mapper;

        public ProductsService(IProductsRepository productsRepository, IMapper mapper)
        {
            this.productsRepository = productsRepository;
            this.mapper = mapper;
        }

        public async Task<string> AddProductAsync(CreateProductDTO createProductDTO)
        {
            var response = await productsRepository.AddProductAsync(mapper.Map<Product>(createProductDTO));

            if (response == true)
            {
                return null;
            }
            return "Product added successfully!";
        }

        public async Task<ProductDTO> GetProductById(int id)
        {
            var response = await productsRepository.GetProductByIdAsync(id);

            return mapper.Map<ProductDTO>(response);
        }

        public async Task<List<ProductDTO>> GetProductsAsync()
        {
            var products = await productsRepository.GetAllProductsAsync();

            return mapper.Map<List<ProductDTO>>(products);
        }

        public async Task<string> UpdateProductAsync(int id, UpdateProductDTO updateProductDTO)
        {
            var response = await productsRepository.UpdateProductAsync(id, mapper.Map<Product>(updateProductDTO));
            if (response != true) return null;
            return "Product updated successfully!";
        }
    }
}

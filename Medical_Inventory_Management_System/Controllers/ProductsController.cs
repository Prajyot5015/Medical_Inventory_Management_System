using Medical_Inventory_Management_System.Models.DTOs;
using Medical_Inventory_Management_System.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Medical_Inventory_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService productsService;

        public ProductsController(IProductsService productsService)
        {
            this.productsService = productsService;
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(CreateProductDTO createProductDTO)
        {
            var response = await productsService.AddProductAsync(createProductDTO);
            if (response != null)
            {
                return BadRequest("Product could not be added.");
            }
            return Ok(response);
        }

        [HttpGet]

        public async Task<IActionResult> GetAllProducts()
        {
            var response = await productsService.GetProductsAsync();
            if (response == null)
            {
                return NotFound("No products found.");
            }
            return Ok(response);
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetProductById(int id)
        {
            var response = await productsService.GetProductById(id);
            if (response == null)
            {
                return NotFound("Product not found.");
            }
            return Ok(response);
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateProduct(int id, UpdateProductDTO updateProductDTO)
        {
            var response = await productsService.UpdateProductAsync(id, updateProductDTO);
            if (response == null)
            {
                return NotFound("Product not found.");
            }
            return Ok(response);
        }

    }
}

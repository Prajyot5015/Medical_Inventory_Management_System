using Medical_Inventory_Management_System.Models.Domain;
using Medical_Inventory_Management_System.Models.DTOs;
using Medical_Inventory_Management_System.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Medical_Inventory_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandsService brandsService;

        public BrandsController(IBrandsService brandsService)
        {
            this.brandsService = brandsService;
        }

        [HttpGet]

        public async Task<IActionResult> GetAllBrands()
        {
            var response = await brandsService.GetAllBrandsAsync();

            return response != null ? Ok(response) : NotFound("No brands found");
        }

        [HttpGet("{id:int}")]

        public async Task<IActionResult> GetBrandById(int id)
        {
            var response = await brandsService.GetBrandByIdAsync(id);

            if (response == null)
            {
                return NotFound("Brand not found");
            }

            return Ok(response);
        }

        [HttpPost]

        public async Task<IActionResult> AddBrand(CreateBrandDTO createBrandDTO)
        {

            var response = await brandsService.AddBrandAsync(createBrandDTO);

            return response == "Brand added successfully" ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateBrand(int id, UpdateBrandDTO updateBrandDTO)
        {
            var response = await brandsService.UpdateBrandAsync(id, updateBrandDTO);
            return response == "Brand updated successfully" ? Ok(response) : NotFound();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            var brand = await brandsService.DeleteBrandAsync(id);

            return Ok(brand);
        }
    }
}

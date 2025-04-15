using Medical_Inventory_Management_System.Models.DTOs;
using Medical_Inventory_Management_System.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Medical_Inventory_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManufacturersController : ControllerBase
    {
        private readonly IManufacturersService manufacturersService;

        public ManufacturersController(IManufacturersService manufacturersService)
        {
            this.manufacturersService = manufacturersService;
        }

        [HttpPost]

        public async Task<IActionResult> AddManufacture(CreateManufacturersDTO createManufacturersDTO)
        {
            var response = await manufacturersService.AddManufacturersAsync(createManufacturersDTO);

            return response == "Manufacture Added Successfully" ? Ok(response) : BadRequest(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllManufacturers()
        {
            var response = await manufacturersService.GetAllManuFactures();
            return Ok(response);
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetManufacturerById(int id)
        {
            var response = await manufacturersService.GetManufacturerById(id);
            if (response == null) return NotFound("Manufacturer Not Found");
            return Ok(response);
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateManufacturer(int id, UpdateManuFacturerDTO updateManuFacturerDTO)
        {
            var response = await manufacturersService.UpdateManufacturer(id, updateManuFacturerDTO);
            return Ok(response);
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteManufacturer(int id)
        {
            var repsponse = await manufacturersService.DeleteManufacturer(id);
            if (repsponse == false) return NotFound("Manufacturer Not Found");
            return Ok("Manufacturer Deleted Successfully");
        }
    }
}

using Medical_Inventory_Management_System.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Medical_Inventory_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockService _stockService;

        public StockController(IStockService stockService)
        {
            _stockService = stockService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _stockService.GetAllStockDetailsAsync();
            return Ok(result);
        }

        [HttpGet("low-stock")]
        public async Task<IActionResult> GetLowStock()
        {
            var result = await _stockService.GetLowStockAsync();
            return Ok(result);
        }

        [HttpGet("near-expiry")]
        public async Task<IActionResult> GetNearExpiry()
        {
            var result = await _stockService.GetNearExpiryStockAsync();
            return Ok(result);
        }
    }
}

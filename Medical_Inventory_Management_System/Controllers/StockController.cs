using Medical_Inventory_Management_System.Services.Interface;
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

        [HttpPost("update-stock-sale")]
        public async Task<IActionResult> UpdateStockAfterSale(int productId, int quantitySold)
        {
            await _stockService.UpdateStockAfterSaleAsync(productId, quantitySold);
            return Ok("Stock updated successfully");
        }

        [HttpPost("update-stock-purchase")]
        public async Task<IActionResult> UpdateStockAfterPurchase(int productId, int quantityPurchased)
        {
            await _stockService.UpdateStockAfterPurchaseAsync(productId, quantityPurchased);
            return Ok("Stock updated successfully");
        }

        [HttpPost("add-stock")]
        public async Task<IActionResult> AddStockToProduct(int productId, int quantityToAdd)
        {
            try
            {
                await _stockService.AddStockToProductAsync(productId, quantityToAdd);
                return Ok("Stock added successfully");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

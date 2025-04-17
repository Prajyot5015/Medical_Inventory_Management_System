using Medical_Inventory_Management_System.Models.DTOs;
using Medical_Inventory_Management_System.Services;
using Medical_Inventory_Management_System.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Medical_Inventory_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly ISaleService _saleService;

        public SalesController(ISaleService saleService)
        {
            _saleService = saleService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSale([FromBody] CreateSaleDto dto)
        {
            try
            {
                var sale = await _saleService.CreateSaleAsync(dto);
                return Ok(sale);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSales()
        {
            var sales = await _saleService.GetAllSalesAsync();
            return Ok(sales);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSaleById(int id)
        {
            try
            {
                var sale = await _saleService.GetSaleByIdAsync(id);
                return Ok(sale);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("{id}/invoice")]
        public async Task<IActionResult> GetInvoice(int id, [FromServices] InvoiceService invoiceService)
        {
            try
            {
                var sale = await _saleService.GetSaleByIdAsync(id);
                var pdfBytes = invoiceService.GenerateInvoice(sale);

                return File(pdfBytes, "application/pdf", $"Invoice_Sale_{id}.pdf");
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }


    }
}

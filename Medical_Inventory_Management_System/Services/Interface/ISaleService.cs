using Medical_Inventory_Management_System.Models.DTOs;

namespace Medical_Inventory_Management_System.Services.Interface
{
    public interface ISaleService
    {
        Task<SaleResponseDto> CreateSaleAsync(CreateSaleDto dto);
        Task<List<SaleResponseDto>> GetAllSalesAsync();

        Task<SaleResponseDto> GetSaleByIdAsync(int id);

        Task<IEnumerable<SaleResponseDto>> SearchSalesAsync(string query);

    }
}

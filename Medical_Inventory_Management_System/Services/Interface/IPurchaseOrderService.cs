using Medical_Inventory_Management_System.Models.DTOs;

namespace Medical_Inventory_Management_System.Services.Interface
{
    public interface IPurchaseOrderService
    {
        Task<PurchaseOrderDTO> CreateAsync(CreatePurchaseOrderDTO dto);

        Task<List<PurchaseOrderDTO>> GetPurchaseOrderAsync();
    }
}

using Medical_Inventory_Management_System.Models.Domain;

namespace Medical_Inventory_Management_System.Repositories.Interface
{
    public interface IPurchaseOrderRepository
    {
        Task<PurchaseOrder> AddAsync(PurchaseOrder order);
        Task<List<PurchaseOrder>> GetPurchaseOrderAsync();
    }
}

namespace Medical_Inventory_Management_System.Models.DTOs
{
    public class CreatePurchaseOrderDTO
    {
        public string SupplierName { get; set; }
        public List<PurchaseOrderItemDTO> Items { get; set; }
    }
}

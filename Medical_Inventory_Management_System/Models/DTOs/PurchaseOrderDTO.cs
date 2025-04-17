namespace Medical_Inventory_Management_System.Models.DTOs
{
    public class PurchaseOrderDTO
    {
        public int Id { get; set; }
        public string SupplierName { get; set; }
        public DateTime OrderDate { get; set; }
        public List<PurchaseOrderItemDTO> Items { get; set; }
    }
}

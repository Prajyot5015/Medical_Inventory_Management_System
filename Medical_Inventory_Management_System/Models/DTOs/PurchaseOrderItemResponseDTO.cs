namespace Medical_Inventory_Management_System.Models.DTOs
{
    public class PurchaseOrderItemResponseDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal PurchasePrice { get; set; }
    }
}

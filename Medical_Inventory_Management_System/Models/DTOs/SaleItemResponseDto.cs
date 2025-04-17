namespace Medical_Inventory_Management_System.Models.DTOs
{
    public class SaleItemResponseDto
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total => Quantity * UnitPrice;
    }
}

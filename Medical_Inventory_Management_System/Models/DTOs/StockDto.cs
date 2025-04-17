namespace Medical_Inventory_Management_System.Models.DTOs
{
    public class StockDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int CurrentStock { get; set; }
        public int LowStockThreshold { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool IsLowStock { get; set; }
        public bool IsNearExpiry { get; set; }
    }
}

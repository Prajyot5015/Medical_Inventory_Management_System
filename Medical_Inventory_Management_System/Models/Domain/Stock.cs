namespace Medical_Inventory_Management_System.Models.Domain
{
    public class Stock
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int CurrentStock { get; set; }

        public int LowStockThreshold { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public void UpdateStock(int newStockQuantity)
        {
            CurrentStock = newStockQuantity;
        }
    }
}

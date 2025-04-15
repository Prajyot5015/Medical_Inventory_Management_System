namespace Medical_Inventory_Management_System.Models.Domain
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Batch { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Unit { get; set; }
        public decimal Price { get; set; }

        // Foreign Keys
        public int BrandId { get; set; }
        public Brand Brand { get; set; }

        public int ManufacturerId { get; set; }
        public Manufacturer Manufacturer { get; set; }

        // Navigation
        public ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; }
        public ICollection<SalesItem> SalesItems { get; set; }
    }
}

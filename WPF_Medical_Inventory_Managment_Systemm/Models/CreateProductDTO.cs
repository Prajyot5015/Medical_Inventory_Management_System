namespace WPF_Medical_Inventory_Managment_Systemm.Models
{
    public class CreateProductDTO
    {
        public string Name { get; set; }
        public string Batch { get; set; }
        public DateTime ExpiryDate { get; set; } = DateTime.UtcNow;
        public string Unit { get; set; }
        public string Price { get; set; }

        public int Stock { get; set; }
        public int BrandId { get; set; }
        public int ManufacturerId { get; set; }
    }
}

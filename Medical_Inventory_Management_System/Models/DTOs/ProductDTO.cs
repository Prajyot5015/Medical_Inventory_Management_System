using Medical_Inventory_Management_System.Models.Domain;

namespace Medical_Inventory_Management_System.Models.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Batch { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Unit { get; set; }
        public decimal Price { get; set; }

        public BrandDTO Brand { get; set; }
        public ManufacturersDTO Manufacturer { get; set; }
    }
}

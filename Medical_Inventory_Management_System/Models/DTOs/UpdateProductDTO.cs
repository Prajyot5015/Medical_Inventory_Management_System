namespace Medical_Inventory_Management_System.Models.DTOs
{
    public class UpdateProductDTO
    {
        public string Name { get; set; }
        public string Batch { get; set; }
        public string Unit { get; set; }
        public decimal Price { get; set; }

        public int BrandId { get; set; }
        public int ManufacturerId { get; set; }
    }
}

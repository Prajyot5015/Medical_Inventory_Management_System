namespace Medical_Inventory_Management_System.Models.Domain
{
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Navigation
        public ICollection<Product> Products { get; set; }
    }
}

namespace Medical_Inventory_Management_System.Models.Domain
{
    public class Manufacturer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContactDetails { get; set; }
        public string Address { get; set; }

        // Navigation
        public ICollection<Product> Products { get; set; }
    }
}

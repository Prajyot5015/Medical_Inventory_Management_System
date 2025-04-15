namespace Medical_Inventory_Management_System.Models.Domain
{
    public class Sale
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public DateTime SaleDate { get; set; }

        // Navigation
        public ICollection<SalesItem> Items { get; set; }
    }
}

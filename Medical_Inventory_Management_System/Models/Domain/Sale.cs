namespace Medical_Inventory_Management_System.Models.Domain
{
    public class Sale
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public DateTime SaleDate { get; set; }

        public decimal TotalAmount { get; set; }    
        public decimal Discount { get; set; }       
        public decimal GrandTotal { get; set; }      

        // Navigation
        public ICollection<SalesItem> Items { get; set; }
    }
}

namespace Medical_Inventory_Management_System.Models.Domain
{
    public class PurchaseOrder
    {
        public int Id { get; set; }
        public string SupplierName { get; set; }
        public DateTime OrderDate { get; set; }

        // Navigation
        public ICollection<PurchaseOrderItem> Items { get; set; }
    }
}

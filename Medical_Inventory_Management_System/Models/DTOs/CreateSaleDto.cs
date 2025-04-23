namespace Medical_Inventory_Management_System.Models.DTOs
{
    public class CreateSaleDto
    {
        public string CustomerName { get; set; }
        public List<SaleItemDto> Items { get; set; }

        public decimal DiscountPercentage { get; set; }
    }
}

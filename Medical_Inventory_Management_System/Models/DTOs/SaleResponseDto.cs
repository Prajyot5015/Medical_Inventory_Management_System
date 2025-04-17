namespace Medical_Inventory_Management_System.Models.DTOs
{
    public class SaleResponseDto
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public DateTime SaleDate { get; set; }
        public List<SaleItemResponseDto> Items { get; set; }
    }
}

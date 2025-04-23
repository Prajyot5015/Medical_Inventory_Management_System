using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Medical_Inventory_Managment_Systemm.Models
{
    public class SaleResponseDto
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public DateTime SaleDate { get; set; }
        public List<SaleItemResponseDto> Items { get; set; }

        //public decimal TotalAmount
        //{
        //    get
        //    {
        //        return Items?.Sum(item => item.Total) ?? 0;
        //    }
        //}
        public decimal TotalAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal GrandTotal { get; set; }
    }
}

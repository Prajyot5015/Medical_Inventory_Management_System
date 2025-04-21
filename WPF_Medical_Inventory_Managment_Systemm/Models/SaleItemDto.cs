using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Medical_Inventory_Managment_Systemm.Models
{
    public class SaleItemDto
    {
        public int ProductId { get; set; }           
        public string Name { get; set; }            
        public int Quantity { get; set; }           
        public decimal UnitPrice { get; set; }      
        public decimal Total { get; set; }
    }
}

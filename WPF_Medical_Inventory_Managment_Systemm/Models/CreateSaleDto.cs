using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Medical_Inventory_Managment_Systemm.Models
{
    public class CreateSaleDto
    {
        public string CustomerName { get; set; }
        public List<SaleItemDto> Items { get; set; } = new();
    }
}

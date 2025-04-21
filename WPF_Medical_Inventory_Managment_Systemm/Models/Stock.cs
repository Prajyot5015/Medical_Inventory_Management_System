using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Medical_Inventory_Managment_Systemm.Models
{
    public class Stock
    {
        public int Id { get; set; }
        public int CurrentStock { get; set; }
        public int LowStockThreshold { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public Product Product { get; set; }

    }
}

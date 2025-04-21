using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Medical_Inventory_Managment_Systemm.Models
{
    public class StockDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Batch { get; set; }
        public int CurrentStock { get; set; }
        public int LowStockThreshold { get; set; }
        public DateTime? ExpiryDate { get; set; }


        public string BrandName { get; set; }
        public string ManufacturerName { get; set; }

    }

}

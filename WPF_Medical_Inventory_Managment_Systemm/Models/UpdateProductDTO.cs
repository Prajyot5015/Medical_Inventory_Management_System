using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Medical_Inventory_Managment_Systemm.Models
{
    public class UpdateProductDTO
    {
        public string Name { get; set; }
        public string Batch { get; set; }
        public string Unit { get; set; }
        public decimal Price { get; set; }

        public int BrandId { get; set; }
        public int ManufacturerId { get; set; }
    }
}

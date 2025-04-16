using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Medical_Inventory_Managment_Systemm.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Batch { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Unit { get; set; }
        public decimal Price { get; set; }
        public BrandDTO Brand { get; set; }
        public ManufacturersDTO Manufacturer { get; set; }
    }
}

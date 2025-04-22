using System;
using System.Collections.Generic;

namespace WPF_Medical_Inventory_Managment_Systemm.Models
{
    public class PurchaseOrderDTO
    {
        public int Id { get; set; }
        public string SupplierName { get; set; }
        public DateTime OrderDate { get; set; }

        // SelectedProduct will be used in WPF for dropdown binding
        public Product SelectedProduct { get; set; }

        public int Quantity { get; set; }
        public decimal PurchasePrice { get; set; }
    }

    public class PurchaseOrderItem
    {
        public int Id { get; set; }
        public int PurchaseOrderId { get; set; }
        public int ProductId { get; set; }

        public Product Product { get; set; }

        public int Quantity { get; set; }
        public decimal PurchasePrice { get; set; }
    }
}



//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace WPF_Medical_Inventory_Managment_Systemm.Models
//{
//    public class PurchaseOrderDTO
//    {
//        public int Id { get; set; }
//        public string SupplierName { get; set; }
//        public DateTime OrderDate { get; set; }
//        public Product SelectedProduct { get; set; } // Single product for the order
//        public int Quantity { get; set; }
//        public decimal PurchasePrice { get; set; }
//    }

//    public class PurchaseOrderItem
//    {
//        public int Id { get; set; }
//        public int PurchaseOrderId { get; set; }
//        public int ProductId { get; set; }
//        public Product Product { get; set; }
//        public int Quantity { get; set; }
//        public decimal PurchasePrice { get; set; }
//    }
//}



//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace WPF_Medical_Inventory_Managment_Systemm.Models
//{

//    public class PurchaseOrderDTO
//    {
//        public int Id { get; set; }
//        public string SupplierName { get; set; }
//        public DateTime OrderDate { get; set; }
//        public List<PurchaseOrderItem> Items { get; set; }
//    }

//    public class PurchaseOrderItem
//    {
//        public int Id { get; set; }
//        public int PurchaseOrderId { get; set; }
//        public int ProductId { get; set; }
//        public Product Product { get; set; }
//        public int Quantity { get; set; }
//        public decimal PurchasePrice { get; set; }
//    }

//}


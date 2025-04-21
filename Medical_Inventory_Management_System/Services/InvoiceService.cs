using Medical_Inventory_Management_System.Models.DTOs;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Linq;

namespace Medical_Inventory_Management_System.Services
{
    public class InvoiceService
    {
        public byte[] GenerateInvoice(SaleResponseDto sale)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Header().Row(row =>
                    {
                        row.RelativeItem().AlignCenter().Text("Medical Inventory Management System").FontSize(14).Bold();
                        row.RelativeItem().AlignRight().Text($"Invoice - Sale #{sale.Id}").FontSize(16).SemiBold();
                    });

                    page.Content().Column(col =>
                    {
                        col.Spacing(15);  // Increased spacing for better layout
                        col.Item().AlignLeft().Text($"Customer: {sale.CustomerName}");
                        col.Item().AlignLeft().Text($"Date: {sale.SaleDate:dd-MM-yyyy}");
                        col.Item().AlignLeft().Text($"Invoice Date: {DateTime.Now:dd-MM-yyyy}");
                        col.Item().AlignLeft().Text("-------------------------------------------------------------");

                        col.Item().AlignLeft().Text("Company Information:");
                        col.Item().AlignLeft().Text("XYZ Medical Supplies");
                        col.Item().AlignLeft().Text("123 Health St, Medical City");
                        col.Item().AlignLeft().Text("Phone: +1234567890");
                        col.Item().AlignLeft().Text("Email: info@xyzmedicals.com");
                        col.Item().AlignLeft().Text("-------------------------------------------------------------");

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(cols =>
                            {
                                cols.RelativeColumn();
                                cols.ConstantColumn(60);
                                cols.ConstantColumn(80);
                                cols.ConstantColumn(80);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Padding(5).Text("Product").SemiBold().AlignStart();  
                                header.Cell().Padding(5).Text("Qty").SemiBold().AlignCenter();  
                                header.Cell().Padding(5).Text("Price (₹)").SemiBold().AlignCenter(); 
                                header.Cell().Padding(5).Text("Total (₹)").SemiBold().AlignCenter(); 
                            });

                            foreach (var item in sale.Items)
                            {
                                table.Cell().Padding(5).Text(item.ProductName).AlignLeft();  
                                table.Cell().Padding(5).Text(item.Quantity.ToString()).AlignCenter(); 
                                table.Cell().Padding(5).Text($"₹{item.UnitPrice:F2}").AlignRight();  
                                table.Cell().Padding(5).Text($"₹{item.Total:F2}").AlignRight();  
                            }

                            table.Cell().ColumnSpan(3).Padding(5).AlignRight().Text("Grand Total:").SemiBold();  
                            table.Cell().Padding(5).Text($"₹{sale.Items.Sum(i => i.Total):F2}").SemiBold().AlignRight();  
                        });

                        col.Item().AlignRight().Text("-------------------------------------------------------------");
                        col.Item().AlignRight().Text($"Total Amount: ₹{sale.Items.Sum(i => i.Total):F2}").FontSize(14).Bold();
                        col.Item().AlignRight().Text("-------------------------------------------------------------");
                        col.Item().AlignRight().Text("Thank you for your business!").FontSize(12);
                    });

                    page.Footer().Row(row =>
                    {
                        row.RelativeItem().AlignLeft().Text($"Invoice generated on {DateTime.Now:dd-MM-yyyy}");
                        row.RelativeItem().AlignRight().Text("XYZ Medical Supplies - All Rights Reserved");
                    });
                });
            });

            return document.GeneratePdf();
        }
    }
}













//using Medical_Inventory_Management_System.Models.DTOs;
//using QuestPDF.Fluent;


//namespace Medical_Inventory_Management_System.Services
//{
//    public class InvoiceService
//    {
//        public byte[] GenerateInvoice(SaleResponseDto sale)
//        {
//            var document = Document.Create(container =>
//            {
//                container.Page(page =>
//                {
//                    page.Margin(30);
//                    page.Header().Text($"Invoice - Sale #{sale.Id}").FontSize(20).SemiBold().AlignCenter();
//                    page.Content().Column(col =>
//                    {
//                        col.Spacing(10);
//                        col.Item().Text($"Customer: {sale.CustomerName}");
//                        col.Item().Text($"Date: {sale.SaleDate:dd-MM-yyyy}");

//                        col.Item().Table(table =>
//                        {
//                            table.ColumnsDefinition(cols =>
//                            {
//                                cols.RelativeColumn();
//                                cols.ConstantColumn(60);
//                                cols.ConstantColumn(80);
//                                cols.ConstantColumn(80);
//                            });

//                            table.Header(header =>
//                            {
//                                header.Cell().Text("Product").SemiBold();
//                                header.Cell().Text("Qty").SemiBold();
//                                header.Cell().Text("Price").SemiBold();
//                                header.Cell().Text("Total").SemiBold();
//                            });

//                            foreach (var item in sale.Items)
//                            {
//                                table.Cell().Text(item.ProductName);
//                                table.Cell().Text(item.Quantity.ToString());
//                                table.Cell().Text($"₹{item.UnitPrice:F2}");
//                                table.Cell().Text($"₹{item.Total:F2}");
//                            }

//                            table.Cell().ColumnSpan(3).AlignRight().Text("Grand Total:").SemiBold();
//                            table.Cell().Text($"₹{sale.Items.Sum(i => i.Total):F2}").SemiBold();
//                        });
//                    });
//                });
//            });

//            return document.GeneratePdf();
//        }
//    }

//}

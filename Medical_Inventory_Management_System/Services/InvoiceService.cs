using Medical_Inventory_Management_System.Models.DTOs;
using QuestPDF.Fluent;


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
                    page.Header().Text($"Invoice - Sale #{sale.Id}").FontSize(20).SemiBold().AlignCenter();
                    page.Content().Column(col =>
                    {
                        col.Spacing(10);
                        col.Item().Text($"Customer: {sale.CustomerName}");
                        col.Item().Text($"Date: {sale.SaleDate:dd-MM-yyyy}");

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
                                header.Cell().Text("Product").SemiBold();
                                header.Cell().Text("Qty").SemiBold();
                                header.Cell().Text("Price").SemiBold();
                                header.Cell().Text("Total").SemiBold();
                            });

                            foreach (var item in sale.Items)
                            {
                                table.Cell().Text(item.ProductName);
                                table.Cell().Text(item.Quantity.ToString());
                                table.Cell().Text($"₹{item.UnitPrice:F2}");
                                table.Cell().Text($"₹{item.Total:F2}");
                            }

                            table.Cell().ColumnSpan(3).AlignRight().Text("Grand Total:").SemiBold();
                            table.Cell().Text($"₹{sale.Items.Sum(i => i.Total):F2}").SemiBold();
                        });
                    });
                });
            });

            return document.GeneratePdf();
        }
    }

}

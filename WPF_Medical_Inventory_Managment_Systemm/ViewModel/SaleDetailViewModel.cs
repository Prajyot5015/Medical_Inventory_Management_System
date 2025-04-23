using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPF_Medical_Inventory_Managment_Systemm.Helpers.RelayCommands;
using WPF_Medical_Inventory_Managment_Systemm.Models;
using WPF_Medical_Inventory_Managment_Systemm.Services;

namespace WPF_Medical_Inventory_Managment_Systemm.ViewModel
{
    public class SaleDetailViewModel : INotifyPropertyChanged
    {
        private readonly SalesApiService _salesApiService;

        public SaleResponseDto Sale { get; set; }

        public int Id => Sale.Id;  
        public string CustomerName => Sale.CustomerName;  
        public decimal TotalAmount => Sale.TotalAmount;  
        public decimal Discount => Sale.Discount; 
        public decimal GrandTotal => Sale.GrandTotal;  
        public DateTime SaleDate => Sale.SaleDate;  

       
        public List<SaleItemResponseDto> Items => Sale.Items;  

        public ICommand ReprintCommand { get; }

        public SaleDetailViewModel(SalesApiService apiService, SaleResponseDto sale)
        {
            _salesApiService = apiService;
            Sale = sale;

            ReprintCommand = new RelayCommand(async () => await DownloadInvoiceAsync(Sale.Id));
        }

        private async Task DownloadInvoiceAsync(int saleId)
        {
            string invoiceUrl = $"https://localhost:7228/api/Sales/{saleId}/invoice";

            var pdfResponse = await _salesApiService.GetInvoiceAsync(invoiceUrl);

            if (pdfResponse != null)
            {
                var invoicesFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Invoices");
                Directory.CreateDirectory(invoicesFolderPath);

                var filePath = Path.Combine(invoicesFolderPath, $"Invoice_{saleId}.pdf");
                File.WriteAllBytes(filePath, pdfResponse);

                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(filePath) { UseShellExecute = true });
            }
            else
            {
                MessageBox.Show("Failed to download invoice.");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }

}

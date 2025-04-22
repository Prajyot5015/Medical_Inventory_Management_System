using System.Collections.ObjectModel;
using System.ComponentModel;
using WPF_Medical_Inventory_Managment_Systemm.Models;

namespace WPF_Medical_Inventory_Managment_Systemm.ViewModel
{
    public class SaleDetailsViewModel : INotifyPropertyChanged
    {
        public SaleResponseDto Sale { get; set; }

        public SaleDetailsViewModel(SaleResponseDto sale)
        {
            Sale = sale;
        }

        public string CustomerName => Sale.CustomerName;
        public string SaleDate => Sale.SaleDate.ToString("yyyy-MM-dd HH:mm");
        public decimal TotalAmount => Sale.TotalAmount;

        public ObservableCollection<SaleItemResponseDto> SaleItems =>
            new ObservableCollection<SaleItemResponseDto>(Sale.Items);

        public event PropertyChangedEventHandler PropertyChanged;
    }


}

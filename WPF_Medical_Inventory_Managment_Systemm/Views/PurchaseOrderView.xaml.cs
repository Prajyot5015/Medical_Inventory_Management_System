using System.Windows.Controls;

namespace WPF_Medical_Inventory_Managment_Systemm.Views
{
    public partial class PurchaseOrderView : Page
    {
        public PurchaseOrderView()
        {
            InitializeComponent();
            DataContext = new ViewModel.PurchaseOrderViewModel();
        }
    }
}

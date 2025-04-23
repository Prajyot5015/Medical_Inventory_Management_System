using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPF_Medical_Inventory_Managment_Systemm.Models;
using WPF_Medical_Inventory_Managment_Systemm.Services;
using WPF_Medical_Inventory_Managment_Systemm.ViewModel;

namespace WPF_Medical_Inventory_Managment_Systemm.Views
{
    /// <summary>
    /// Interaction logic for SaleDetailsWindow.xaml
    /// </summary>
    public partial class SaleDetailsWindow : Window
    {
        public SaleDetailsWindow(SaleResponseDto sale)
        {
            InitializeComponent();
            var viewModel = new SaleDetailViewModel(new SalesApiService(), sale);
            DataContext = viewModel;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ReprintButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

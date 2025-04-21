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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF_Medical_Inventory_Managment_Systemm.ViewModels;

namespace WPF_Medical_Inventory_Managment_Systemm.Views
{
    /// <summary>
    /// Interaction logic for StockView.xaml
    /// </summary>
    public partial class StockView : Page
    {
        private readonly StockViewModel _viewModel;

        public StockView()
        {
            InitializeComponent();
            _viewModel = new StockViewModel();
            DataContext = _viewModel;

            Loaded += async (s, e) =>
            {
                await _viewModel.LoadStockData();
                await _viewModel.LoadLowStockData();
                await _viewModel.LoadNearExpiryData();
            };
        }

        private void LowStockDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void NearExpiryDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

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
using System.Windows.Media.Animation;
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
            this.Loaded += StockView_Loaded;


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
       
        private void NearExpiryDataGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }

        private void MainTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void Tab_Loaded(object sender, RoutedEventArgs e)
        {
            var element = sender as FrameworkElement;
            var storyboard = (Storyboard)Resources["FadeInStoryboard"];
            storyboard.Begin(element);
        }

        private void StockView_Loaded(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as StockViewModel;

            if (vm != null && !vm.PopupShownOnce)
            {
                PopupOverlay.Visibility = Visibility.Visible;
                vm.PopupShownOnce = true;
            }
        }

    }
}

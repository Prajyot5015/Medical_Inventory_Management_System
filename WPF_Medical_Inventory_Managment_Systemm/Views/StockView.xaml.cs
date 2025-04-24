using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using WPF_Medical_Inventory_Managment_Systemm.ViewModels;
using WPF_Medical_Inventory_Managment_Systemm.Helpers; // Add this for SessionState

namespace WPF_Medical_Inventory_Managment_Systemm.Views
{
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
                try
                {
                    await _viewModel.LoadStockData();
                    await _viewModel.LoadLowStockData();
                    await _viewModel.LoadNearExpiryData();

                    if (!SessionState.IsLowStockPopupShown &&
                        _viewModel.LowStockList != null &&
                        _viewModel.LowStockList.Any())
                    {
                        LowStockPopup.Visibility = Visibility.Visible;
                        SessionState.IsLowStockPopupShown = true; // Mark as shown
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading data: {ex.Message}", "Load Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            };
        }

        private void LowStockDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e) { }

        private void NearExpiryDataGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e) { }

        private void MainTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e) { }

        private void Tab_Loaded(object sender, RoutedEventArgs e)
        {
            var element = sender as FrameworkElement;
            var storyboard = (Storyboard)Resources["FadeInStoryboard"];
            storyboard.Begin(element);
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            LowStockPopup.Visibility = Visibility.Collapsed;
            MainTabControl.SelectedIndex = 1;  // Show "Low Stock" tab
        }

        private void ClosePopup_Click(object sender, RoutedEventArgs e)
        {
            LowStockPopup.Visibility = Visibility.Collapsed;
        }
    }
}

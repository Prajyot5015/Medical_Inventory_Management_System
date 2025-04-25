using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using WPF_Medical_Inventory_Managment_Systemm.ViewModel;

namespace WPF_Medical_Inventory_Managment_Systemm.Views
{
    public partial class BrandView : Page
    {
        private readonly BrandViewModel _viewModel;

        public BrandView()
        {
            InitializeComponent();
            _viewModel = new BrandViewModel();
            DataContext = _viewModel;

            this.Loaded += BrandView_Loaded;
        }

        private async void BrandView_Loaded(object sender, RoutedEventArgs e)
        {
            if (_viewModel != null)
                await _viewModel.LoadAsync();
        }
        private void BrandNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DataContext is BrandViewModel viewModel)
            {
                viewModel.MarkManualEntry();
            }
        }

        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {

            try
            {
                if (Resources["SlideDownStoryboard"] is Storyboard slideDownStoryboard)
                {
                    // Set the target to the DataGrid's TranslateTransform
                    Storyboard.SetTargetName(slideDownStoryboard, "DataGridTranslateTransform");
                    slideDownStoryboard.Begin();
                }
            }
            catch (Exception ex)
            {
                // Log or handle the error appropriately
                System.Diagnostics.Debug.WriteLine($"Error in DataGrid_Loaded: {ex.Message}");
            }
        }



    }
}

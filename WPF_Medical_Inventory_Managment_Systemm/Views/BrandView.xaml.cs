using System.Windows;
using System.Windows.Controls;
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
            await _viewModel.LoadAsync();
        }
    }
}

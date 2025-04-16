using System.Windows;
using WPF_Medical_Inventory_Managment_Systemm.ViewModel;

namespace WPF_Medical_Inventory_Managment_Systemm.Views
{
    public partial class BrandView : Window
    {
        private readonly BrandViewModel _viewModel;

        public BrandView()
        {
            InitializeComponent();
            _viewModel = new BrandViewModel();
            DataContext = _viewModel;

            Loaded += async (_, __) => await _viewModel.LoadAsync();
        }
    }
}

using System.Windows;
using WPF_Medical_Inventory_Managment_Systemm.ViewModel;

namespace WPF_Medical_Inventory_Managment_Systemm.Views
{
    public partial class ManufacturerView : Window
    {
        private readonly ManufacturerViewModel _viewModel;

        public ManufacturerView()
        {
            InitializeComponent();
            _viewModel = new ManufacturerViewModel();
            DataContext = _viewModel;

            Loaded += async (_, __) => await _viewModel.LoadAsync();
        }
    }
}

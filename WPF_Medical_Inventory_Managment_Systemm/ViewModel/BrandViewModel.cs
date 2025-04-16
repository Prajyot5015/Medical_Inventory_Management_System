using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPF_Medical_Inventory_Managment_Systemm.Models;
using WPF_Medical_Inventory_Managment_Systemm.Services;
using WPF_Medical_Inventory_Managment_Systemm.Helpers;

namespace WPF_Medical_Inventory_Managment_Systemm.ViewModel
{
    public class BrandViewModel : INotifyPropertyChanged
    {
        private readonly BrandService _service;

        public ObservableCollection<Brand> Brands { get; } = new();

        private Brand _selectedBrand = new Brand();
        public Brand SelectedBrand
        {
            get => _selectedBrand;
            set
            {
                if (_selectedBrand != value)
                {
                    _selectedBrand = value;
                    OnPropertyChanged();
                    ((RelayCommand)UpdateCommand).RaiseCanExecuteChanged();
                    ((RelayCommand)DeleteCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public ICommand LoadCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }

        public BrandViewModel()
        {
            _service = new BrandService(App.HttpClient);

            LoadCommand = new RelayCommand(async () => await LoadAsync());
            AddCommand = new RelayCommand(async () => await AddAsync());
            UpdateCommand = new RelayCommand(async () => await UpdateAsync(), () => SelectedBrand?.Id > 0);
            DeleteCommand = new RelayCommand(async () => await DeleteAsync(), () => SelectedBrand?.Id > 0);
        }

        public async Task LoadAsync()
        {
            Brands.Clear();
            var brands = await _service.GetBrandsAsync();
            foreach (var brand in brands)
                Brands.Add(brand);
        }

        public async Task AddAsync()
        {
            if (string.IsNullOrWhiteSpace(SelectedBrand.Name))
            {
                MessageBox.Show("Brand name cannot be empty.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            await _service.AddBrandAsync(SelectedBrand);
            SelectedBrand = new Brand(); // reset
            OnPropertyChanged(nameof(SelectedBrand));
            await LoadAsync();
        }

        public async Task UpdateAsync()
        {
            if (string.IsNullOrWhiteSpace(SelectedBrand.Name))
            {
                MessageBox.Show("Brand name cannot be empty.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            await _service.UpdateBrandAsync(SelectedBrand);
            await LoadAsync();
        }

        public async Task DeleteAsync()
        {
            var result = MessageBox.Show($"Are you sure you want to delete '{SelectedBrand.Name}'?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                await _service.DeleteBrandAsync(SelectedBrand.Id);
                SelectedBrand = new Brand(); // reset
                OnPropertyChanged(nameof(SelectedBrand));
                await LoadAsync();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

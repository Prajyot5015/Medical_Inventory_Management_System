
using System;
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
                _selectedBrand = value ?? new Brand();
                OnPropertyChanged();
                ((RelayCommand)UpdateCommand).RaiseCanExecuteChanged();
                ((RelayCommand)DeleteCommand).RaiseCanExecuteChanged();
                ((RelayCommand)AddCommand).RaiseCanExecuteChanged(); // Recheck the AddCommand availability
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
            AddCommand = new RelayCommand(async () => await AddAsync(), () => SelectedBrand?.Id == 0); // Disabled when a brand is selected
            UpdateCommand = new RelayCommand(async () => await UpdateAsync(), () => SelectedBrand?.Id > 0);
            DeleteCommand = new RelayCommand(async () => await DeleteAsync(), () => SelectedBrand?.Id > 0);

            // Load brands on initialization
            _ = LoadAsync(); // Fire-and-forget call
        }

        private bool _isLoaded = false;

        public async Task LoadAsync()
        {
            Brands.Clear();
            var brands = await _service.GetBrandsAsync();
            foreach (var brand in brands)
                Brands.Add(brand);

            _isLoaded = true;
        }

        public async Task AddAsync()
        {
            if (string.IsNullOrWhiteSpace(SelectedBrand.Name))
            {
                MessageBox.Show("Brand name cannot be empty.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                await _service.AddBrandAsync(SelectedBrand);
                MessageBox.Show("Brand added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                SelectedBrand = new Brand();
                await LoadAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to add brand. Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
                try
                {
                    await _service.DeleteBrandAsync(SelectedBrand.Id);
                    MessageBox.Show("Brand deleted successfully.", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                    SelectedBrand = new Brand();
                    await LoadAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to delete brand. Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public async Task RefreshAsync()
        {
            Brands.Clear();  // Clear the brands collection
            await LoadAsync();  // Reload the brands
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}




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
using MaterialDesignThemes.Wpf;
using WPF_Medical_Inventory_Managment_Systemm.Views.Notification;

namespace WPF_Medical_Inventory_Managment_Systemm.ViewModel
{
    public class BrandViewModel : INotifyPropertyChanged
    {
        private readonly BrandService _service;
        private Brand _selectedBrand = new Brand();
        private bool _isManuallyEntered;
        private bool _isLoaded = false;

        public ISnackbarMessageQueue SnackBarMessageQueue { get; }
        public ObservableCollection<Brand> Brands { get; } = new();

        public Brand SelectedBrand
        {
            get => _selectedBrand;
            set
            {
                _selectedBrand = value ?? new Brand();
                IsManuallyEntered = false; // reset when selected from list
                OnPropertyChanged();
                ((RelayCommand)UpdateCommand).RaiseCanExecuteChanged();
                ((RelayCommand)DeleteCommand).RaiseCanExecuteChanged();
                ((RelayCommand)AddCommand).RaiseCanExecuteChanged();
            }
        }

        public ICommand LoadCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }

        public BrandViewModel()
        {
            _service = new BrandService(App.HttpClient);
            SnackBarMessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(3));

            LoadCommand = new RelayCommand(async () => await LoadAsync());
            AddCommand = new RelayCommand(async () => await AddAsync(), () => SelectedBrand?.Id == 0);
            UpdateCommand = new RelayCommand(async () => await UpdateAsync(), () => SelectedBrand?.Id > 0);
            DeleteCommand = new RelayCommand(async () => await DeleteAsync(), () => SelectedBrand?.Id > 0);

            // Load brands on initialization
            _ = LoadAsync();
        }

        public async Task LoadAsync()
        {
            Brands.Clear();
            var brands = await _service.GetBrandsAsync();
            foreach (var brand in brands)
            {
                Brands.Add(brand);
            }
            _isLoaded = true;
        }

        public async Task AddAsync()
        {
            if (string.IsNullOrWhiteSpace(SelectedBrand.Name))
            {
                MessageBox.Show("Brand name cannot be empty.", "Validation Error",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                await _service.AddBrandAsync(SelectedBrand);
                SnackBarMessageQueue.Enqueue("Brand added successfully.");
                SelectedBrand = new Brand();
                await LoadAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to add brand. Error: {ex.Message}",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async Task UpdateAsync()
        {
            if (string.IsNullOrWhiteSpace(SelectedBrand.Name))
            {
                MessageBox.Show("Brand name cannot be empty.", "Validation Error",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            await _service.UpdateBrandAsync(SelectedBrand);
            SnackBarMessageQueue.Enqueue("Brand Updated Successfully");
            await LoadAsync();
        }

        public async Task DeleteAsync()
        {
            if (SelectedBrand == null) return;

            try
            {
                var brandToDelete = SelectedBrand;
                await _service.DeleteBrandAsync(brandToDelete.Id);

                // Remove the specific item instead of reloading
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Brands.Remove(brandToDelete);
                });

                SnackBarMessageQueue.Enqueue("Brand deleted successfully");
                SelectedBrand = new Brand();
            }
            catch (Exception ex)
            {
                SnackBarMessageQueue.Enqueue($"Delete failed: {ex.Message}");
            }
        }


        public bool IsManuallyEntered
        {
            get => _isManuallyEntered;
            set
            {
                _isManuallyEntered = value;
                Console.WriteLine($"IsManuallyEntered set to: {_isManuallyEntered}");
                OnPropertyChanged();
            }
        }

        public void MarkManualEntry()
        {
            if (SelectedBrand != null && !string.IsNullOrWhiteSpace(SelectedBrand.Name))
            {
                bool isExisting = false;
                foreach (var brand in Brands)
                {
                    if (string.Equals(brand.Name, SelectedBrand.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        isExisting = true;
                        break;
                    }
                }
                IsManuallyEntered = !isExisting;
            }
        }

        public async Task RefreshAsync()
        {
            Brands.Clear();
            await LoadAsync();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPF_Medical_Inventory_Managment_Systemm.Helpers.RelayCommands;
using WPF_Medical_Inventory_Managment_Systemm.Models;
using WPF_Medical_Inventory_Managment_Systemm.Services;

namespace WPF_Medical_Inventory_Managment_Systemm.ViewModels
{
    public class StockViewModel : INotifyPropertyChanged
    {
        private readonly StockApiService _stockApiService;
        private readonly ProductApiService _productApiService;
        private readonly BrandService _brandService;
        private readonly ManufacturersApiService _manufacturerService;

        public StockViewModel()
        {
            _stockApiService = new StockApiService();
            _productApiService = new ProductApiService();
            _brandService = new BrandService(new HttpClient { BaseAddress = new Uri("http://your-api-url/") });
            _manufacturerService = new ManufacturersApiService(new HttpClient { BaseAddress = new Uri("http://your-api-url/") });

            LoadStockDataCommand = new RelayCommand(async () => await LoadStockData());
            LoadLowStockCommand = new RelayCommand(async () => await LoadLowStockData());
            LoadNearExpiryCommand = new RelayCommand(async () => await LoadNearExpiryData());
            AddStockCommand = new RelayCommand(async () => await AddStock());
            NavigateToAddStockCommand = new RelayCommand(() => NavigateToAddStock(SelectedStockItem));
            ResetFormCommand = new RelayCommand(ResetForm);
            CloseAddStockTabCommand = new RelayCommand(ExecuteCloseAddStockTab);

            _ = LoadStockData();
            _ = LoadProducts();
        }

        public ICommand LoadStockDataCommand { get; }
        public ICommand LoadLowStockCommand { get; }
        public ICommand LoadNearExpiryCommand { get; }
        public ICommand UpdateStockAfterSaleCommand { get; }
        public ICommand UpdateStockAfterPurchaseCommand { get; }
        public ICommand NavigateToAddStockCommand { get; }
        public ICommand ResetFormCommand { get; }
        public ICommand CloseAddStockTabCommand { get; }
        public ICommand AddStockCommand { get; }


        private void ResetForm()
        {
            SelectedProductId = 0; // or null if you're using nullable int
            QuantityToAdd = null;  // works only if QuantityToAdd is int?
        }

        private ObservableCollection<StockDto> _stockList;
        public ObservableCollection<StockDto> StockList
        {
            get => _stockList;
            set { _stockList = value; OnPropertyChanged(); }
        }

        private ObservableCollection<StockDto> _lowStockList;
        public ObservableCollection<StockDto> LowStockList
        {
            get => _lowStockList;
            set { _lowStockList = value; OnPropertyChanged(); }
        }

        private ObservableCollection<StockDto> _nearExpiryStockList;
        public ObservableCollection<StockDto> NearExpiryStockList
        {
            get => _nearExpiryStockList;
            set { _nearExpiryStockList = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Product> _products = new ObservableCollection<Product>();
        public ObservableCollection<Product> Products
        {
            get => _products;
            set { _products = value; OnPropertyChanged(); }
        }

        private async Task LoadProducts()
        {
            try
            {
                var productList = await _productApiService.GetAllProductsAsync();
                Products = new ObservableCollection<Product>(productList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load products: {ex.Message}");
            }
        }

        private int _selectedTabIndex;
        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set
            {
                _selectedTabIndex = value;
                OnPropertyChanged();
                _ = HandleTabChangeAsync(value);
            }
        }

        private async Task HandleTabChangeAsync(int index)
        {
            switch (index)
            {
                case 0: await LoadStockData(); break;
                case 1: await LoadLowStockData(); break;
                case 2: await LoadNearExpiryData(); break;
            }
        }

        private StockDto _selectedStockItem;
        public StockDto SelectedStockItem
        {
            get => _selectedStockItem;
            set { _selectedStockItem = value; OnPropertyChanged(); }
        }



        private bool _showLowStockAlert;
        public bool ShowLowStockAlert
        {
            get => _showLowStockAlert;
            set
            {
                _showLowStockAlert = value;
                OnPropertyChanged();
            }
        }






        public async Task LoadStockData()
        {
            var stocks = await _stockApiService.GetAllStockAsync();
            var stockDtos = new ObservableCollection<StockDto>();

            var allBrands = await _brandService.GetBrandsAsync();
            var allManufacturers = await _manufacturerService.GetAllManufacturersAsync();

            foreach (var stock in stocks)
            {
                var product = stock.ProductId > 0 ? await _productApiService.GetProductByIdAsync(stock.ProductId) : null;

                string brandName = "Unknown";
                string manufacturerName = "Unknown";

                if (product != null)
                {
                    var brand = allBrands.FirstOrDefault(b => b.Id == product.Brand?.Id);
                    var manufacturer = allManufacturers.FirstOrDefault(m => m.Id == product.Manufacturer?.Id);

                    brandName = brand?.Name ?? "Unknown";
                    manufacturerName = manufacturer?.Name ?? "Unknown";
                }

                stockDtos.Add(new StockDto
                {
                    Id = stock.Id,
                    ProductId = stock.ProductId,
                    ProductName = product?.Name ?? stock.ProductName ?? "Unknown",
                    Batch = stock.Batch,
                    CurrentStock = stock.CurrentStock,
                    LowStockThreshold = stock.LowStockThreshold,
                    ExpiryDate = stock.ExpiryDate,
                    BrandName = brandName,
                    ManufacturerName = manufacturerName
                });
            }

            StockList = stockDtos;
        }

        public async Task LoadLowStockData()
        {
            var stocks = await _stockApiService.GetLowStockAsync();
            var stockDtos = new ObservableCollection<StockDto>();

            var allBrands = await _brandService.GetBrandsAsync();
            var allManufacturers = await _manufacturerService.GetAllManufacturersAsync();

            foreach (var stock in stocks)
            {
                var product = stock.ProductId > 0 ? await _productApiService.GetProductByIdAsync(stock.ProductId) : null;

                string brandName = "Unknown";
                string manufacturerName = "Unknown";

                if (product != null)
                {
                    var brand = allBrands.FirstOrDefault(b => b.Id == product.Brand?.Id);
                    var manufacturer = allManufacturers.FirstOrDefault(m => m.Id == product.Manufacturer?.Id);

                    brandName = brand?.Name ?? "Unknown";
                    manufacturerName = manufacturer?.Name ?? "Unknown";
                }

                stockDtos.Add(new StockDto
                {
                    Id = stock.Id,
                    ProductId = stock.ProductId,
                    ProductName = product?.Name ?? stock.ProductName ?? "Unknown",
                    Batch = stock.Batch,
                    CurrentStock = stock.CurrentStock,
                    LowStockThreshold = stock.LowStockThreshold,
                    ExpiryDate = stock.ExpiryDate,
                    BrandName = brandName,
                    ManufacturerName = manufacturerName
                });
            }

            LowStockList = stockDtos;
        }

        public async Task LoadNearExpiryData()
        {
            var stocks = await _stockApiService.GetNearExpiryStockAsync();
            var stockDtos = new ObservableCollection<StockDto>();

            var allBrands = await _brandService.GetBrandsAsync();
            var allManufacturers = await _manufacturerService.GetAllManufacturersAsync();

            foreach (var stock in stocks)
            {
                var product = stock.ProductId > 0 ? await _productApiService.GetProductByIdAsync(stock.ProductId) : null;

                string brandName = "Unknown";
                string manufacturerName = "Unknown";

                if (product != null)
                {
                    var brand = allBrands.FirstOrDefault(b => b.Id == product.Brand?.Id);
                    var manufacturer = allManufacturers.FirstOrDefault(m => m.Id == product.Manufacturer?.Id);

                    brandName = brand?.Name ?? "Unknown";
                    manufacturerName = manufacturer?.Name ?? "Unknown";
                }

                stockDtos.Add(new StockDto
                {
                    Id = stock.Id,
                    ProductId = stock.ProductId,
                    ProductName = product?.Name ?? stock.ProductName ?? "Unknown",
                    Batch = stock.Batch,
                    CurrentStock = stock.CurrentStock,
                    LowStockThreshold = stock.LowStockThreshold,
                    ExpiryDate = stock.ExpiryDate,
                    BrandName = brandName,
                    ManufacturerName = manufacturerName
                });
            }

            NearExpiryStockList = stockDtos;
        }

        private int _selectedProductId;
        public int SelectedProductId
        {
            get => _selectedProductId;
            set
            {
                _selectedProductId = value;
                OnPropertyChanged();
                (AddStockCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        private int? _quantityToAdd;
        public int? QuantityToAdd
        {
            get => _quantityToAdd;
            set
            {
                _quantityToAdd = value;
                OnPropertyChanged(nameof(QuantityToAdd));
            }
        }

        private async Task AddStock()
        {
            try
            {
                if (!QuantityToAdd.HasValue || QuantityToAdd <= 0)
                {
                    MessageBox.Show("Please enter a valid positive number");
                    return;
                }

                await _stockApiService.AddStockToProductAsync(SelectedProductId, QuantityToAdd.Value);
                MessageBox.Show("Stock added successfully.");
                QuantityToAdd = null; // Clear the input after successful addition
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void ExecuteCloseAddStockTab()
        {
            SelectedTabIndex = _previousTabIndex; // Replace 0 with the index you want to navigate back to (e.g. All Stock)
        }

        private int _previousTabIndex;
        private void NavigateToAddStock(StockDto selectedItem)
        {
            if (selectedItem != null)
            {
                SelectedProductId = selectedItem.ProductId;
                _previousTabIndex = SelectedTabIndex;  // store current tab before navigating
                SelectedTabIndex = 3; // Index for "Add Stock" tab
            }
        }

        public bool HasLowStockItems()
    {
        return LowStockList != null && LowStockList.Any();
    }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
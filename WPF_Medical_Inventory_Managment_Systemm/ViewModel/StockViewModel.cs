using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
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
            UpdateStockAfterSaleCommand = new RelayCommand(async () => await UpdateStockAfterSale());
            UpdateStockAfterPurchaseCommand = new RelayCommand(async () => await UpdateStockAfterPurchase());
            AddStockCommand = new RelayCommand(async () => await AddStock());
        }

        public ICommand LoadStockDataCommand { get; }
        public ICommand LoadLowStockCommand { get; }
        public ICommand LoadNearExpiryCommand { get; }
        public ICommand UpdateStockAfterSaleCommand { get; }
        public ICommand UpdateStockAfterPurchaseCommand { get; }
        public ICommand AddStockCommand { get; }

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

        private int _selectedTabIndex;
        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set { _selectedTabIndex = value; OnPropertyChanged(); }
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

        public async Task UpdateStockAfterSale()
        {
            await _stockApiService.UpdateStockAfterSaleAsync(1, 10); // Replace with real values
        }

        private async Task UpdateStockAfterPurchase()
        {
            await _stockApiService.UpdateStockAfterPurchaseAsync(1, 10); // Replace with real values
        }

        private async Task AddStock()
        {
            await _stockApiService.AddStockToProductAsync(1, 50); // Replace with real values
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

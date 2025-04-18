using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using WPF_Medical_Inventory_Managment_Systemm.Helpers.RelayCommands;
using WPF_Medical_Inventory_Managment_Systemm.Models;
using WPF_Medical_Inventory_Managment_Systemm.Services;

namespace WPF_Medical_Inventory_Managment_Systemm.ViewModel
{
    public class ProductViewModel : INotifyPropertyChanged
    {
        private readonly ProductApiService _apiService;
        private readonly BrandService _brandService;
        private readonly ManufacturersApiService _manufacturerService;

        public Product SelectedProduct { get; set; }
        public CreateProductDTO NewProduct { get; set; } = new();

        public ObservableCollection<Brand> Brands { get; set; } = new();
        public ObservableCollection<ManufacturersDTO> Manufacturers { get; set; } = new();
        public ObservableCollection<Product> Products { get; set; } = new();

        public ICommand LoadCommand { get; }
        public ICommand AddProductCommand { get; }
        public ICommand EditProductCommand { get; }
        public ICommand UpdateProductCommand { get; }
        public ICommand DeleteProductCommand { get; }

        public ProductViewModel()
        {
            _apiService = new ProductApiService();
            _brandService = new BrandService(new HttpClient { BaseAddress = new Uri("https://localhost:7228/api/") });
            _manufacturerService = new ManufacturersApiService(new HttpClient { BaseAddress = new Uri("https://localhost:7228/api/") });

            LoadCommand = new RelayCommand(async () => await LoadData());
            AddProductCommand = new RelayCommand(async () => await AddProduct());
            EditProductCommand = new RelayCommand<Product>(async (product) => await EditProduct(product));
            UpdateProductCommand = new RelayCommand(async () => await UpdateProduct());
            DeleteProductCommand = new RelayCommand<Product>(async (product) => await DeleteProduct(product));
        }

        // Load all necessary data: Products, Brands, Manufacturers
        private async Task LoadData()
        {
            var products = await _apiService.GetAllProductsAsync();
            var brands = await _brandService.GetBrandsAsync();
            var manufacturers = await _manufacturerService.GetAllManufacturersAsync();

            Products.Clear();
            foreach (var p in products) Products.Add(p);

            Brands.Clear();
            foreach (var b in brands) Brands.Add(b);

            Manufacturers.Clear();
            foreach (var m in manufacturers) Manufacturers.Add(m);
        }

        // Add a new product
        private async Task AddProduct()
        {
            if (decimal.TryParse(NewProduct.Price, out decimal parsedPrice))
            {
                NewProduct.Price = parsedPrice.ToString("0.00");
            }
            else
            {
                NewProduct.Price = "0.00";
            }

            // Ensure Brand and Manufacturer are selected
            if (NewProduct.BrandId == 0 || NewProduct.ManufacturerId == 0)
                return;

            var success = await _apiService.CreateProductAsync(NewProduct);
            if (success)
            {
                await LoadData();
                ResetForm();
            }
        }

        // Edit selected product to populate form for update
        private async Task EditProduct(Product product)
        {
            if (product == null) return;

            SelectedProduct = product;
            NewProduct = new CreateProductDTO
            {
                Name = product.Name,
                Batch = product.Batch,
                Unit = product.Unit,
                Price = product.Price.ToString("0.00"),
                Stock = product.Stock,
                BrandId = product.Brand?.Id ?? 0,
                ManufacturerId = product.Manufacturer?.Id ?? 0,
                ExpiryDate = product.ExpiryDate
            };

            OnPropertyChanged(nameof(NewProduct));
        }

        // Update the selected product
        private async Task UpdateProduct()
        {
            if (SelectedProduct == null) return;

            var dto = new UpdateProductDTO
            {
                Name = NewProduct.Name,
                Batch = NewProduct.Batch,
                Unit = NewProduct.Unit,
                Price = decimal.TryParse(NewProduct.Price, out var p) ? p : 0,
                Stock = NewProduct.Stock,
                BrandId = NewProduct.BrandId,
                ManufacturerId = NewProduct.ManufacturerId
            };

            var success = await _apiService.UpdateProductAsync(SelectedProduct.Id, dto);
            if (success)
            {
                await LoadData();
                ResetForm();
            }
        }

        // Delete a product
        private async Task DeleteProduct(Product product)
        {
            if (product == null) return;

            var success = await _apiService.DeleteProductAsync(product.Id);
            if (success)
            {
                await LoadData();
                ResetForm();
            }
        }

        // Reset the form after CRUD operation
        private void ResetForm()
        {
            NewProduct = new CreateProductDTO();
            SelectedProduct = null;
            OnPropertyChanged(nameof(NewProduct));
        }

        // INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}




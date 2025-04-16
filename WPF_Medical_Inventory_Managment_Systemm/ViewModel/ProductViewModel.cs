using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
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
        //private readonly ManufacturerService _manufacturerService;

        public ObservableCollection<Brand> Brands { get; set; } = new();
      //  public ObservableCollection<Manufacturer> Manufacturers { get; set; } = new();
        public ObservableCollection<Product> Products { get; set; } = new();
        public CreateProductDTO NewProduct { get; set; } = new();

        public ICommand LoadCommand { get; }
        public ICommand AddProductCommand { get; }

        public ProductViewModel()
        {
            _apiService = new ProductApiService();
            LoadCommand = new RelayCommand(async () => await LoadProducts());
            LoadCommand = new RelayCommand(async () =>  await LoadData());
            AddProductCommand = new RelayCommand(async () => await AddProduct());

            _brandService = new BrandService(new HttpClient { BaseAddress = new Uri("https://localhost:7228/api/") });
        //    _manufacturerService = new ManufacturerService(new HttpClient { BaseAddress = new Uri("https://localhost:7228/api/") });

        }

        // Load All Products
        private async Task LoadProducts()
        {
            var products = await _apiService.GetAllProductsAsync();
            // Products.Clear();
            foreach (var product in products)
            {
                Products.Add(product);
            }
        }


        private async Task LoadData()
        {
            var products = await _apiService.GetAllProductsAsync();
            var brands = await _brandService.GetBrandsAsync();
           // var manufacturers = await _manufacturerService.GetManufacturersAsync();

            Products.Clear();
            foreach (var p in products) Products.Add(p);

            Brands.Clear();
            foreach (var b in brands) Brands.Add(b);

           // Manufacturers.Clear();
          //  foreach (var m in manufacturers) Manufacturers.Add(m);
        }

        // Add New Product
        private async Task AddProduct()
        {
            // Convert Price from string to decimal
            if (decimal.TryParse(NewProduct.Price, out decimal parsedPrice))
            {
                NewProduct.Price = parsedPrice.ToString("0.00");
            }
            else
            {
                NewProduct.Price = "0.00"; 
            }
            NewProduct.ManufacturerId = 1;
            var success = await _apiService.CreateProductAsync(NewProduct);  
            if (success)
            {
                await LoadProducts();
                await LoadData();
                NewProduct = new CreateProductDTO(); 
                OnPropertyChanged(nameof(NewProduct));
            }
        }


        // PropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    }
}

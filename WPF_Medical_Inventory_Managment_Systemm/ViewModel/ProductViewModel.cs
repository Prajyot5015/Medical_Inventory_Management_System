﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Windows;
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

        public ICommand ConfirmDeleteCommand { get; }
        public ICommand CancelDeleteCommand { get; }

        private Product _productToDelete;

        private bool _isProductAddedPopupVisible;
        public string ProductAddedPopupVisibility => _isProductAddedPopupVisible ? "Visible" : "Collapsed";

        private bool _isUpdateConfirmationPopupVisible;
        public string UpdateConfirmationPopupVisibility => _isUpdateConfirmationPopupVisible ? "Visible" : "Collapsed";

        private bool _isUpdateSuccessPopupVisible;
        public string UpdateSuccessPopupVisibility => _isUpdateSuccessPopupVisible ? "Visible" : "Collapsed";

        public ICommand ConfirmUpdateCommand => new RelayCommand(async () => await ConfirmUpdate());
        public ICommand CancelUpdateCommand => new RelayCommand(CancelUpdate);




        public ProductViewModel()
        {
            _apiService = new ProductApiService();
            _brandService = new BrandService(new HttpClient { BaseAddress = new Uri("https://localhost:7228/api/") });
            _manufacturerService = new ManufacturersApiService(new HttpClient { BaseAddress = new Uri("https://localhost:7228/api/") });

            LoadCommand = new RelayCommand(async () => await LoadData());
            AddProductCommand = new RelayCommand(async () => await AddProduct());
            EditProductCommand = new RelayCommand<Product>(async (product) => await EditProduct(product));
            UpdateProductCommand = new RelayCommand(async () => await UpdateProduct());

            DeleteProductCommand = new RelayCommand<Product>(async (product) => await PrepareDeleteProduct(product));

            ConfirmDeleteCommand = new RelayCommand(async () => await ConfirmDelete());
            CancelDeleteCommand = new RelayCommand(CancelDelete);
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

            ValidateProduct();

            if (HasValidationErrors())
            {
                return; // Don't proceed if validation fails
            }


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
                await ShowProductAddedPopup();
                // MessageBox.Show("Product added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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
            ValidateProduct();

            if (HasValidationErrors())
                return;

            if (SelectedProduct == null) return;

           ShowUpdateConfirmationPopup(); 
        }

        private async Task ConfirmUpdate()
        {
            HideUpdateConfirmationPopup(); // hide the confirmation popup first

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
                await ShowUpdateSuccessPopup(); // show success popup
                await LoadData();
                ResetForm();
            }
        }

        private void CancelUpdate()
        {
            HideUpdateConfirmationPopup();
        }



        // Delete a product
        private async Task DeleteProduct(Product product)
        {
            if (product == null) return;

            var result = MessageBox.Show("Are you sure you want to delete this product?",
                                "Confirm Deletion",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                var success = await _apiService.DeleteProductAsync(product.Id);
                if (success)
                {
                    await LoadData();
                    ResetForm();
                }
            }
        }

        // Reset the form after CRUD operation
        private void ResetForm()
        {
            NewProduct = new CreateProductDTO();
            SelectedProduct = null;
            OnPropertyChanged(nameof(NewProduct));
        }

        public string NameError { get; set; }
        public string BatchError { get; set; }
        public string UnitError { get; set; }
        public string PriceError { get; set; }
        public string BrandError { get; set; }
        public string ManufacturerError { get; set; }
        public string QuantityError { get; set; }

        // Validation method
        private void ValidateProduct()
        {
            NameError = string.IsNullOrEmpty(NewProduct.Name) ? "Please enter product name." : null;
            BatchError = string.IsNullOrEmpty(NewProduct.Batch) ? "Please enter batch number." : null;
            UnitError = string.IsNullOrEmpty(NewProduct.Unit) ? "Please enter unit." : null;
            PriceError = decimal.TryParse(NewProduct.Price, out _) ? null : "Please enter a valid price.";
            BrandError = NewProduct.BrandId == 0 ? "Please select a brand." : null;
            ManufacturerError = NewProduct.ManufacturerId == 0 ? "Please select a manufacturer." : null;
            QuantityError = NewProduct.Stock <= 0 ? "Please enter a valid quantity." : null;


            OnPropertyChanged(nameof(NameError));
            OnPropertyChanged(nameof(BatchError));
            OnPropertyChanged(nameof(UnitError));
            OnPropertyChanged(nameof(PriceError));
            OnPropertyChanged(nameof(BrandError));
            OnPropertyChanged(nameof(ManufacturerError));
            OnPropertyChanged(nameof(QuantityError));
        }

        // Check if any validation error exists
        private bool HasValidationErrors()
        {
            return !string.IsNullOrEmpty(NameError) ||
                   !string.IsNullOrEmpty(BatchError) ||
                   !string.IsNullOrEmpty(UnitError) ||
                   !string.IsNullOrEmpty(PriceError) ||
                    !string.IsNullOrEmpty(BrandError) ||
                   !string.IsNullOrEmpty(ManufacturerError) ||
                   !string.IsNullOrEmpty(QuantityError);
        }



        private async Task PrepareDeleteProduct(Product product)
        {
            if (product == null) return;

            _productToDelete = product;

            // Show the confirmation popup
            OnPropertyChanged(nameof(DeleteConfirmationPopupVisibility)); // Notify the view to show the popup
        }

        private async Task ConfirmDelete()
        {
            if (_productToDelete == null) return;

            var success = await _apiService.DeleteProductAsync(_productToDelete.Id);
            if (success)
            {
                await LoadData();
                ResetForm();
            }


            _productToDelete = null;
            // Hide the confirmation popup
            OnPropertyChanged(nameof(DeleteConfirmationPopupVisibility)); // Hide the popup after deletion
        }

        private void CancelDelete()
        {
            _productToDelete = null;

            // Hide the confirmation popup
            OnPropertyChanged(nameof(DeleteConfirmationPopupVisibility)); // Hide the popup
        }

        // Property for controlling the visibility of the popup in the view
        public string DeleteConfirmationPopupVisibility => _productToDelete == null ? "Collapsed" : "Visible";

        private async Task ShowProductAddedPopup()
        {
            _isProductAddedPopupVisible = true;
            OnPropertyChanged(nameof(ProductAddedPopupVisibility));

            await Task.Delay(2000); // Show popup for 2 seconds

            _isProductAddedPopupVisible = false;
            OnPropertyChanged(nameof(ProductAddedPopupVisibility));
        }

        private void ShowUpdateConfirmationPopup()
        {
            _isUpdateConfirmationPopupVisible = true;
            OnPropertyChanged(nameof(UpdateConfirmationPopupVisibility));
        }

        private void HideUpdateConfirmationPopup()
        {
            _isUpdateConfirmationPopupVisible = false;
            OnPropertyChanged(nameof(UpdateConfirmationPopupVisibility));
        }

        private async Task ShowUpdateSuccessPopup()
        {
            _isUpdateSuccessPopupVisible = true;
            OnPropertyChanged(nameof(UpdateSuccessPopupVisibility));

            await Task.Delay(2000); // Show for 2 seconds

            _isUpdateSuccessPopupVisible = false;
            OnPropertyChanged(nameof(UpdateSuccessPopupVisibility));
        }




        // INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}




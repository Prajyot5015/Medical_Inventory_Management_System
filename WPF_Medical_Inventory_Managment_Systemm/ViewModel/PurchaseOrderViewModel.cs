using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPF_Medical_Inventory_Managment_Systemm.Helpers;
using WPF_Medical_Inventory_Managment_Systemm.Models;
using WPF_Medical_Inventory_Managment_Systemm.Services;

namespace WPF_Medical_Inventory_Managment_Systemm.ViewModel
{
    public class PurchaseOrderViewModel : INotifyPropertyChanged
    {
        private readonly PurchaseOrderApiService _orderService;
        private readonly ProductApiService _productService;

        public ObservableCollection<Product> Products { get; set; } = new();
        public ObservableCollection<PurchaseOrderDTO> PurchaseOrders { get; set; } = new();

        private PurchaseOrderDTO _purchaseOrder = new();
        public PurchaseOrderDTO PurchaseOrder
        {
            get => _purchaseOrder;
            set
            {
                _purchaseOrder = value;
                OnPropertyChanged();
                UpdateViewModelPropertiesFromDTO();
            }
        }

        private PurchaseOrderDTO _selectedPurchaseOrder;
        public PurchaseOrderDTO SelectedPurchaseOrder
        {
            get => _selectedPurchaseOrder;
            set
            {
                _selectedPurchaseOrder = value;
                OnPropertyChanged();

                if (_selectedPurchaseOrder != null)
                {
                    PurchaseOrder = new PurchaseOrderDTO
                    {
                        Id = _selectedPurchaseOrder.Id,
                        SupplierName = _selectedPurchaseOrder.SupplierName,
                        OrderDate = _selectedPurchaseOrder.OrderDate,
                        Quantity = _selectedPurchaseOrder.Quantity
                    };

                    SelectedProduct = _selectedPurchaseOrder.Items?.FirstOrDefault()?.Product;
                    PurchasePrice = SelectedProduct?.Price ??0;
                }
            }
        }

        // Now handled in the ViewModel
        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged();
                PurchasePrice = _selectedProduct?.Price ?? 0;
            }
        }

        private decimal _purchasePrice;
        public decimal PurchasePrice
        {
            get => _purchasePrice;
            set
            {
                _purchasePrice = value;
                OnPropertyChanged();
            }
        }

        public ICommand SavePurchaseOrderCommand { get; }
        public ICommand NewPurchaseOrderCommand { get; }

        public PurchaseOrderViewModel()
        {
            _orderService = new PurchaseOrderApiService(App.HttpClient);
            _productService = new ProductApiService(App.HttpClient);

            SavePurchaseOrderCommand = new RelayCommand(async () => await SavePurchaseOrderAsync());
            NewPurchaseOrderCommand = new RelayCommand(() => ResetPurchaseOrder());

            _ = LoadAsync();
        }

        private async Task LoadAsync()
        {
            await LoadProductsAsync();
            await LoadPurchaseOrdersAsync();
        }

        private async Task LoadProductsAsync()
        {
            Products.Clear();
            var products = await _productService.GetAllProductsAsync();
            foreach (var product in products)
                Products.Add(product);
        }

        private async Task LoadPurchaseOrdersAsync()
        {
            PurchaseOrders.Clear();
            var orders = await _orderService.GetAllPurchaseOrdersAsync();
            foreach (var order in orders)
                PurchaseOrders.Add(order);
        }

        private async Task SavePurchaseOrderAsync()
        {
            if (string.IsNullOrWhiteSpace(PurchaseOrder.SupplierName) || SelectedProduct == null || PurchaseOrder.Quantity <= 0)
            {
                MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var dto = new PurchaseOrderDTO
            {
                SupplierName = PurchaseOrder.SupplierName,
                OrderDate = PurchaseOrder.OrderDate,
                Items = new List<PurchaseOrderItem>
                {
                    new PurchaseOrderItem
                    {
                        ProductId = SelectedProduct.Id,
                        Quantity = PurchaseOrder.Quantity,
                        PurchasePrice = SelectedProduct.Price
                    }
                }
            };

            await _orderService.AddPurchaseOrderAsync(dto);
            ResetPurchaseOrder();
            await LoadPurchaseOrdersAsync();
           

        }

        private void ResetPurchaseOrder()
        {
            var defaultProduct = Products.FirstOrDefault();

            PurchaseOrder = new PurchaseOrderDTO
            {
                OrderDate = DateTime.Now,
                Quantity = 0
            };

            //SelectedProduct = defaultProduct;
            //PurchasePrice = defaultProduct?.Price ?? 0;
        }

        private void UpdateViewModelPropertiesFromDTO()
        {
            // This syncs ViewModel-level properties with the newly set DTO.
            SelectedProduct = null;
            PurchasePrice = 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}










//using System.Collections.ObjectModel;
//using System.ComponentModel;
//using System.Runtime.CompilerServices;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Input;
//using WPF_Medical_Inventory_Managment_Systemm.Helpers;
//using WPF_Medical_Inventory_Managment_Systemm.Models;
//using WPF_Medical_Inventory_Managment_Systemm.Services;

//namespace WPF_Medical_Inventory_Managment_Systemm.ViewModel
//{
//    public class PurchaseOrderViewModel : INotifyPropertyChanged
//    {
//        private readonly PurchaseOrderApiService _orderService;
//        private readonly ProductApiService _productService;

//        public ObservableCollection<Product> Products { get; set; } = new();
//        public ObservableCollection<PurchaseOrderDTO> PurchaseOrders { get; set; } = new();

//        private PurchaseOrderDTO _purchaseOrder = new PurchaseOrderDTO();
//        public PurchaseOrderDTO PurchaseOrder
//        {
//            get => _purchaseOrder;
//            set
//            {
//                if (_purchaseOrder != null)
//                {
//                    _purchaseOrder.PropertyChanged -= OnPurchaseOrderPropertyChanged;
//                }

//                _purchaseOrder = value;
//                OnPropertyChanged();

//                if (_purchaseOrder != null)
//                {
//                    _purchaseOrder.PropertyChanged += OnPurchaseOrderPropertyChanged;
//                }
//            }
//        }


//        private PurchaseOrderDTO _selectedPurchaseOrder;
//        public PurchaseOrderDTO SelectedPurchaseOrder
//        {
//            get => _selectedPurchaseOrder;
//            set
//            {
//                _selectedPurchaseOrder = value;
//                OnPropertyChanged();
//                if (_selectedPurchaseOrder != null)
//                {
//                    PurchaseOrder = new PurchaseOrderDTO
//                    {
//                        Id = _selectedPurchaseOrder.Id,
//                        SupplierName = _selectedPurchaseOrder.SupplierName,
//                        OrderDate = _selectedPurchaseOrder.OrderDate,
//                        Quantity = _selectedPurchaseOrder.Quantity,
//                        // Corrected here to use the actual Price from the Product
//                        //PurchasePrice = _selectedPurchaseOrder.Product?.Price ?? 0,
//                        //SelectedProduct = _selectedPurchaseOrder.Product
//                        PurchasePrice = _selectedPurchaseOrder.SelectedProduct?.Price ?? 0,
//                        SelectedProduct = _selectedPurchaseOrder.SelectedProduct
//                    };
//                }
//            }
//        }

//        public ICommand SavePurchaseOrderCommand { get; }
//        public ICommand NewPurchaseOrderCommand { get; }

//        public PurchaseOrderViewModel()
//        {
//            _orderService = new PurchaseOrderApiService(App.HttpClient);
//            SavePurchaseOrderCommand = new RelayCommand(async () => await SavePurchaseOrderAsync());
//            NewPurchaseOrderCommand = new RelayCommand(() => ResetPurchaseOrder());
//   _productService = new ProductApiService(App.HttpClient);



//            _ = LoadAsync();
//        }




//        private async Task LoadAsync()
//        {
//            await LoadProductsAsync();
//            await LoadPurchaseOrdersAsync();
//        }

//        private async Task LoadProductsAsync()
//        {
//            Products.Clear();
//            var products = await _productService.GetAllProductsAsync();
//            foreach (var product in products)
//                Products.Add(product);
//        }

//        private async Task LoadPurchaseOrdersAsync()
//        {
//            PurchaseOrders.Clear();
//            var orders = await _orderService.GetAllPurchaseOrdersAsync();
//            foreach (var order in orders)
//                PurchaseOrders.Add(order);
//        }

//        //private async Task SavePurchaseOrderAsync()
//        //{
//        //    if (string.IsNullOrWhiteSpace(PurchaseOrder.SupplierName) || PurchaseOrder.SelectedProduct == null || PurchaseOrder.Quantity <= 0)
//        //    {
//        //        MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
//        //        return;
//        //    }

//        //    // Set price based on selected product (corrected property name)
//        //    PurchaseOrder.PurchasePrice = PurchaseOrder.SelectedProduct?.Price ?? 0;

//        //    if (PurchaseOrder.Id == 0)
//        //        await _orderService.AddPurchaseOrderAsync(PurchaseOrder);
//        //    //else
//        //    //    await _orderService.UpdatePurchaseOrderAsync(PurchaseOrder);


//        //    ResetPurchaseOrder();
//        //    await LoadPurchaseOrdersAsync();
//        //}
//        private async Task SavePurchaseOrderAsync()
//        {
//            if (string.IsNullOrWhiteSpace(PurchaseOrder.SupplierName) || PurchaseOrder.SelectedProduct == null || PurchaseOrder.Quantity <= 0)
//            {
//                MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
//                return;
//            }

//            var dto = new PurchaseOrderDTO
//            {
//                SupplierName = PurchaseOrder.SupplierName,
//                OrderDate = PurchaseOrder.OrderDate,
//                Items = new List<PurchaseOrderItem>

//        {
//            new PurchaseOrderItem
//            {
//                ProductId = PurchaseOrder.SelectedProduct.Id,
//                Quantity = PurchaseOrder.Quantity,
//                PurchasePrice = PurchaseOrder.SelectedProduct.Price
//            }
//        }
//            };

//            await _orderService.AddPurchaseOrderAsync(dto);

//            ResetPurchaseOrder();
//            await LoadPurchaseOrdersAsync();
//        }


//        //private void ResetPurchaseOrder()
//        //{
//        //    PurchaseOrder = new PurchaseOrderDTO
//        //    {
//        //        OrderDate = DateTime.Now,

//        //    };
//        //}
//        private void ResetPurchaseOrder()
//        {
//            PurchaseOrder = new PurchaseOrderDTO
//            {
//                OrderDate = DateTime.Now,
//                //SelectedProduct = Products.FirstOrDefault(),
//                PurchasePrice = Products.FirstOrDefault()?.Price ?? 0
//            };
//        }

//        private void OnPurchaseOrderPropertyChanged(object sender, PropertyChangedEventArgs e)
//        {
//            if (e.PropertyName == nameof(PurchaseOrderDTO.SelectedProduct))
//            {
//                PurchaseOrder.PurchasePrice = PurchaseOrder.SelectedProduct?.Price ?? 0;
//            }
//        }


//        public event PropertyChangedEventHandler PropertyChanged;
//        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
//            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
//    }

//}




//using System;
//using System.Collections.ObjectModel;
//using System.ComponentModel;
//using System.Runtime.CompilerServices;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Input;
//using WPF_Medical_Inventory_Managment_Systemm.Helpers;
//using WPF_Medical_Inventory_Managment_Systemm.Models;
//using WPF_Medical_Inventory_Managment_Systemm.Services;

//namespace WPF_Medical_Inventory_Managment_Systemm.ViewModel
//{
//    public class PurchaseOrderViewModel : INotifyPropertyChanged
//    {
//        private readonly PurchaseOrderApiService _orderService;
//        private readonly ProductApiService _productService;

//        public ObservableCollection<Product> Products { get; set; } = new();
//        public ObservableCollection<PurchaseOrderDTO> PurchaseOrders { get; set; } = new();

//        private PurchaseOrderDTO _purchaseOrder = new PurchaseOrderDTO();
//        public PurchaseOrderDTO PurchaseOrder
//        {
//            get => _purchaseOrder;
//            set
//            {
//                _purchaseOrder = value;
//                OnPropertyChanged();
//            }
//        }

//        private PurchaseOrderDTO _selectedPurchaseOrder;
//        public PurchaseOrderDTO SelectedPurchaseOrder
//        {
//            get => _selectedPurchaseOrder;
//            set
//            {
//                _selectedPurchaseOrder = value;
//                OnPropertyChanged();
//                if (_selectedPurchaseOrder != null)
//                {
//                    PurchaseOrder = new PurchaseOrderDTO
//                    {
//                        Id = _selectedPurchaseOrder.Id,
//                        SupplierName = _selectedPurchaseOrder.SupplierName,
//                        OrderDate = _selectedPurchaseOrder.OrderDate,
//                        Quantity = _selectedPurchaseOrder.Quantity,
//                        PurchasePrice = _selectedPurchaseOrder.PurchasePrice,
//                        SelectedProduct = _selectedPurchaseOrder.Product
//                    };
//                }
//            }
//        }

//        public ICommand SavePurchaseOrderCommand { get; }
//        public ICommand NewPurchaseOrderCommand { get; }

//        public PurchaseOrderViewModel()
//        {
//            _orderService = new PurchaseOrderApiService(App.HttpClient);
//            _productService = new ProductApiService(App.HttpClient);

//            SavePurchaseOrderCommand = new RelayCommand(async () => await SavePurchaseOrderAsync());
//            NewPurchaseOrderCommand = new RelayCommand(() => ResetPurchaseOrder());

//            _ = LoadAsync();
//        }

//        private async Task LoadAsync()
//        {
//            await LoadProductsAsync();
//            await LoadPurchaseOrdersAsync();
//        }

//        private async Task LoadProductsAsync()
//        {
//            Products.Clear();
//            var products = await _productService.GetAllProductsAsync();
//            foreach (var product in products)
//                Products.Add(product);
//        }

//        private async Task LoadPurchaseOrdersAsync()
//        {
//            PurchaseOrders.Clear();
//            var orders = await _orderService.GetAllPurchaseOrdersAsync();
//            foreach (var order in orders)
//                PurchaseOrders.Add(order);
//        }

//        private async Task SavePurchaseOrderAsync()
//        {
//            if (string.IsNullOrWhiteSpace(PurchaseOrder.SupplierName) || PurchaseOrder.SelectedProduct == null || PurchaseOrder.Quantity <= 0)
//            {
//                MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
//                return;
//            }

//            // Set price based on selected product (assuming price comes from the product)
//            PurchaseOrder.PurchasePrice = PurchaseOrder.SelectedProduct.PurchasePrice;

//            if (PurchaseOrder.Id == 0)
//                await _orderService.AddPurchaseOrderAsync(PurchaseOrder);
//            else
//                await _orderService.UpdatePurchaseOrderAsync(PurchaseOrder);

//            ResetPurchaseOrder();
//            await LoadPurchaseOrdersAsync();
//        }

//        private void ResetPurchaseOrder()
//        {
//            PurchaseOrder = new PurchaseOrderDTO
//            {
//                OrderDate = DateTime.Now
//            };
//        }

//        public event PropertyChangedEventHandler PropertyChanged;
//        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
//            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
//    }
//}

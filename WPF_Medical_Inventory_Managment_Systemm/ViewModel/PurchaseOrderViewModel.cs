using System.Collections.ObjectModel;
using System.ComponentModel;
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

        private PurchaseOrderDTO _purchaseOrder = new PurchaseOrderDTO();
        public PurchaseOrderDTO PurchaseOrder
        {
            get => _purchaseOrder;
            set
            {
                _purchaseOrder = value;
                OnPropertyChanged();
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
                        Quantity = _selectedPurchaseOrder.Quantity,
                        // Corrected here to use the actual Price from the Product
                        //PurchasePrice = _selectedPurchaseOrder.Product?.Price ?? 0,
                        //SelectedProduct = _selectedPurchaseOrder.Product
                        PurchasePrice = _selectedPurchaseOrder.SelectedProduct?.Price ?? 0,
                        SelectedProduct = _selectedPurchaseOrder.SelectedProduct
                    };
                }
            }
        }

        public ICommand SavePurchaseOrderCommand { get; }
        public ICommand NewPurchaseOrderCommand { get; }

        public PurchaseOrderViewModel()
        {
            _orderService = new PurchaseOrderApiService(App.HttpClient);
            SavePurchaseOrderCommand = new RelayCommand(async () => await SavePurchaseOrderAsync());
            NewPurchaseOrderCommand = new RelayCommand(() => ResetPurchaseOrder());
   _productService = new ProductApiService(App.HttpClient);

            

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
            if (string.IsNullOrWhiteSpace(PurchaseOrder.SupplierName) || PurchaseOrder.SelectedProduct == null || PurchaseOrder.Quantity <= 0)
            {
                MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Set price based on selected product (corrected property name)
            PurchaseOrder.PurchasePrice = PurchaseOrder.SelectedProduct?.Price ?? 0;

            if (PurchaseOrder.Id == 0)
                await _orderService.AddPurchaseOrderAsync(PurchaseOrder);
            //else
            //    await _orderService.UpdatePurchaseOrderAsync(PurchaseOrder);


            ResetPurchaseOrder();
            await LoadPurchaseOrdersAsync();
        }

        private void ResetPurchaseOrder()
        {
            PurchaseOrder = new PurchaseOrderDTO
            {
                OrderDate = DateTime.Now
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}




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

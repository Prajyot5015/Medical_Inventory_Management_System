using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPF_Medical_Inventory_Managment_Systemm.Helpers.RelayCommands;
using WPF_Medical_Inventory_Managment_Systemm.Models;
using WPF_Medical_Inventory_Managment_Systemm.Services;
using WPF_Medical_Inventory_Managment_Systemm.Views;

public class SalesViewModel : INotifyPropertyChanged
{
    private readonly ProductApiService _productService = new ProductApiService();
    private readonly SalesApiService _saleService = new SalesApiService();

    public ObservableCollection<Product> Products { get; set; } = new ObservableCollection<Product>();
    public ObservableCollection<SaleItemDto> CartItems { get; set; } = new ObservableCollection<SaleItemDto>();
    public ObservableCollection<SaleItemDto> SaleItems { get; set; } = new ObservableCollection<SaleItemDto>();
    public ObservableCollection<SaleResponseDto> AllSales { get; set; } = new ObservableCollection<SaleResponseDto>();

    public ICommand AddToCartCommand { get; }
    public ICommand SubmitSaleCommand { get; }
    public ICommand LoadAllSalesCommand { get; }
    public ICommand SearchSaleByIdCommand { get; }
    public ICommand ShowGenerateSaleViewCommand { get; }
    public ICommand ShowAllSalesViewCommand { get; }
    public ICommand ViewSaleByIdCommand { get; }

    public ICommand IncreaseQuantityCommand { get; }
    public ICommand DecreaseQuantityCommand { get; }

    private bool _productErrorPopupVisibility;
    public bool ProductErrorPopupVisibility
    {
        get => _productErrorPopupVisibility;
        set => SetProperty(ref _productErrorPopupVisibility, value);
    }

    private bool _warningPopupVisibility;
    public bool WarningPopupVisibility
    {
        get => _warningPopupVisibility;
        set => SetProperty(ref _warningPopupVisibility, value);
    }

    private string _errorMessage;
    public string ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }


    public SalesViewModel()
    {
        AddToCartCommand = new RelayCommand(AddToCart);
        SubmitSaleCommand = new RelayCommand(async () => await SubmitSale());
        LoadAllSalesCommand = new RelayCommand(async () => await LoadAllSales());
        SearchSaleByIdCommand = new RelayCommand(async () => await GetSaleById());

        ShowGenerateSaleViewCommand = new RelayCommand(() => IsGenerateSaleViewVisible = true);
        ShowAllSalesViewCommand = new RelayCommand(async () =>
        {
            IsGenerateSaleViewVisible = false;
            await LoadAllSales();
        });

        ViewSaleByIdCommand = new RelayCommand<int>(async (saleId) => await ViewSaleById(saleId));
        IncreaseQuantityCommand = new RelayCommand(IncreaseQuantity);
        DecreaseQuantityCommand = new RelayCommand(DecreaseQuantity);


        LoadProducts();
    }

    private void IncreaseQuantity()
    {
        Quantity += 1;
    }

    private void DecreaseQuantity()
    {
        if (Quantity > 1)
            Quantity -= 1;
    }

    private async void LoadProducts()
    {
        var productList = await _productService.GetAllProductsAsync();
        Products.Clear();
        foreach (var product in productList)
            Products.Add(product);
    }

    private async void AddToCart()
    {
        ValidateCustomerName();

        if (SelectedProduct == null)
        {
            ProductErrorPopupVisibility = true; 

            await Task.Delay(3000);
            ProductErrorPopupVisibility = false; 

            return;
        }

        var unitPrice = SelectedProduct.Price;
        var total = unitPrice * Quantity;

        CartItems.Add(new SaleItemDto
        {
            ProductId = SelectedProduct.Id,
            Quantity = Quantity
        });

        SaleItems.Add(new SaleItemDto
        {
            ProductId = SelectedProduct.Id,
            Quantity = Quantity,
            Name = SelectedProduct.Name,
            UnitPrice = unitPrice,
            Total = total
        });

        CalculateTotalAmount();
        Quantity = 1;
        SelectedProduct = null;
    }

    private void CalculateTotalAmount()
    {
        TotalAmount = SaleItems.Sum(item => item.Total);
        CalculateGrandTotal();
    }

    private void CalculateTotalAmountDiscount()
    {
        var subtotal = SaleItems.Sum(item => item.Total);
        var discountAmount = subtotal * (DiscountPercentage / 100);
        TotalAmount = subtotal - discountAmount;
        CalculateGrandTotal();
    }

    private void CalculateGrandTotal()
    {
        GrandTotalAmount = TotalAmount - (TotalAmount * (OverallDiscountPercentage / 100));
    }

    private async Task SubmitSale()
    {
        // Check for invalid customer name or empty cart
        if (string.IsNullOrWhiteSpace(CustomerName) || CartItems.Count == 0)
        {
            ErrorMessage = "Please enter Customer name and add at least one item to the cart.";
            WarningPopupVisibility = true;
            await Task.Delay(3000); 
            WarningPopupVisibility = false; 
            return;
        }

        // Proceed with sale creation if validation passes
        var sale = new CreateSaleDto
        {
            CustomerName = CustomerName,
            Items = CartItems.ToList(),
            DiscountPercentage = OverallDiscountPercentage
        };

        // Call service to create the sale
        var result = await _saleService.CreateSaleAsync(sale);
        if (result != null)
        {
            await DownloadInvoice(result.Id);
            ResetForm(); 
        }
        else
        {
            ErrorMessage = "Sale creation failed,";
            WarningPopupVisibility = true;
            await Task.Delay(3000); 
            WarningPopupVisibility = false; 
        }
    }
    private void ResetForm()
    {
        CartItems.Clear();
        SaleItems.Clear();
        CustomerName = string.Empty;
        Quantity = 1;
        SelectedProduct = null;
        TotalAmount = 0;
        OverallDiscountPercentage = 0;
        GrandTotalAmount = 0;
    }

    private async Task DownloadInvoice(int saleId)
    {
        var invoiceUrl = $"https://localhost:7228/api/Sales/{saleId}/invoice";
        var pdfResponse = await _saleService.GetInvoiceAsync(invoiceUrl);

        if (pdfResponse != null)
        {
            var invoicesFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Invoices");
            Directory.CreateDirectory(invoicesFolderPath);

            var filePath = Path.Combine(invoicesFolderPath, $"Invoice_{saleId}.pdf");
            File.WriteAllBytes(filePath, pdfResponse);
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(filePath) { UseShellExecute = true });
        }
        else
        {
            MessageBox.Show("Failed to download invoice.");
        }
    }

    private async Task LoadAllSales()
    {
        AllSales.Clear();
        var sales = await _saleService.GetAllSalesAsync();
        foreach (var sale in sales)
            AllSales.Add(sale);
    }

    private async Task GetSaleById()
    {
        if (SearchSaleId > 0)
        {
            SearchedSale = await _saleService.GetSaleByIdAsync(SearchSaleId);
        }
    }

    private async Task ViewSaleById(int saleId)
    {
        var sale = await _saleService.GetSaleByIdAsync(saleId);
        if (sale != null)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var window = new SaleDetailsWindow(sale)
                {
                    Owner = Application.Current.MainWindow 
                };
                window.ShowDialog();
            });
        }
        else
        {
            MessageBox.Show("Sale not found.");
        }
    }

    private void ValidateCustomerName()
    {
        if (string.IsNullOrEmpty(CustomerName))
        {
            CustomerNameError = "Please enter customer name.";
        }
        else
        {
            CustomerNameError = null; 
        }
    }

    private string _customerName;
    public string CustomerName
    {
        get => _customerName;
        set
        {
            SetProperty(ref _customerName, value);
            ValidateCustomerName();
        }
    }

    private string _customerNameError;
    public string CustomerNameError
    {
        get { return _customerNameError; }
        set
        {
            _customerNameError = value;
            OnPropertyChanged(nameof(CustomerNameError));
        }
    }

    private string _productError;

    private Product _selectedProduct;
    public Product SelectedProduct
    {
        get => _selectedProduct;
        set => SetProperty(ref _selectedProduct, value);
    }

    private int _quantity = 1;
    public int Quantity
    {
        get => _quantity;
        set => SetProperty(ref _quantity, value);
    }

    private decimal _discountPercentage;
    public decimal DiscountPercentage
    {
        get => _discountPercentage;
        set
        {
            if (SetProperty(ref _discountPercentage, value))
                CalculateTotalAmountDiscount();
        }
    }

    private decimal _overallDiscountPercentage = 0;
    public decimal OverallDiscountPercentage
    {
        get => _overallDiscountPercentage;
        set
        {
            if (SetProperty(ref _overallDiscountPercentage, value))
                CalculateGrandTotal();
        }
    }

    private decimal _totalAmount;
    public decimal TotalAmount
    {
        get => _totalAmount;
        set => SetProperty(ref _totalAmount, value);
    }

    private decimal _grandTotalAmount;
    public decimal GrandTotalAmount
    {
        get => _grandTotalAmount;
        set => SetProperty(ref _grandTotalAmount, value);
    }

    private int _searchSaleId;
    public int SearchSaleId
    {
        get => _searchSaleId;
        set => SetProperty(ref _searchSaleId, value);
    }

    private SaleResponseDto _searchedSale;
    public SaleResponseDto SearchedSale
    {
        get => _searchedSale;
        set => SetProperty(ref _searchedSale, value);
    }

    private bool _isGenerateSaleViewVisible = true;
    public bool IsGenerateSaleViewVisible
    {
        get => _isGenerateSaleViewVisible;
        set
        {
            if (SetProperty(ref _isGenerateSaleViewVisible, value))
                OnPropertyChanged(nameof(IsAllSalesViewVisible));
        }
    }

    public bool IsAllSalesViewVisible => !IsGenerateSaleViewVisible;

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    protected bool SetProperty<T>(ref T backingField, T value, string propertyName = null)
    {
        if (Equals(backingField, value)) return false;
        backingField = value;
        OnPropertyChanged(propertyName ?? new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Name.Substring(4));
        return true;
    }
}













//using System.Collections.ObjectModel;
//using System.ComponentModel;
//using System.Windows.Input;
//using System.Windows;
//using WPF_Medical_Inventory_Managment_Systemm.Services;
//using WPF_Medical_Inventory_Managment_Systemm.Models;
//using System.IO;
//using WPF_Medical_Inventory_Managment_Systemm.Helpers.RelayCommands;
//using WPF_Medical_Inventory_Managment_Systemm.Views;

//public class SalesViewModel : INotifyPropertyChanged
//{
//    private readonly ProductApiService _productService = new ProductApiService();
//    private readonly SalesApiService _saleService = new SalesApiService();

//    public ObservableCollection<Product> Products { get; set; } = new ObservableCollection<Product>();
//    public ObservableCollection<SaleItemDto> CartItems { get; set; } = new ObservableCollection<SaleItemDto>();
//    public ObservableCollection<SaleItemDto> SaleItems { get; set; } = new ObservableCollection<SaleItemDto>();
//    private string _customerName;
//    public string CustomerName
//    {
//        get => _customerName;
//        set
//        {
//            _customerName = value;
//            OnPropertyChanged(nameof(CustomerName));
//        }
//    }

//    private Product _selectedProduct;
//    public Product SelectedProduct
//    {
//        get => _selectedProduct;
//        set
//        {
//            _selectedProduct = value;
//            OnPropertyChanged(nameof(SelectedProduct));
//        }
//    }

//    private int _quantity = 1;
//    public int Quantity
//    {
//        get => _quantity;
//        set
//        {
//            _quantity = value;
//            OnPropertyChanged(nameof(Quantity));
//        }
//    }


//    private decimal _discountPercentage;
//    public decimal DiscountPercentage
//    {
//        get => _discountPercentage;
//        set
//        {
//            _discountPercentage = value;
//            OnPropertyChanged(nameof(DiscountPercentage));
//            CalculateTotalAmountDiscount(); // Recalculate total when discount changes
//        }
//    }
//    private decimal _overallDiscountPercentage = 0;
//    public decimal OverallDiscountPercentage
//    {
//        get => _overallDiscountPercentage;
//        set
//        {
//            _overallDiscountPercentage = value;
//            OnPropertyChanged(nameof(OverallDiscountPercentage));
//            CalculateGrandTotal();
//        }
//    }

//    private decimal _grandTotalAmount;
//    public decimal GrandTotalAmount
//    {
//        get => _grandTotalAmount;
//        set
//        {
//            _grandTotalAmount = value;
//            OnPropertyChanged(nameof(GrandTotalAmount));
//        }
//    }

//    private void CalculateGrandTotal()
//    {
//        GrandTotalAmount = TotalAmount - (TotalAmount * (OverallDiscountPercentage / 100));
//    }

//    private decimal _totalAmount;
//    public decimal TotalAmount
//    {
//        get => _totalAmount;
//        set
//        {
//            _totalAmount = value;
//            OnPropertyChanged(nameof(TotalAmount));
//        }
//    }

//    public ObservableCollection<SaleResponseDto> AllSales { get; set; } = new ObservableCollection<SaleResponseDto>();

//    private int _searchSaleId;
//    public int SearchSaleId
//    {
//        get => _searchSaleId;
//        set
//        {
//            _searchSaleId = value;
//            OnPropertyChanged(nameof(SearchSaleId));
//        }
//    }

//    private SaleResponseDto _searchedSale;
//    public SaleResponseDto SearchedSale
//    {
//        get => _searchedSale;
//        set
//        {
//            _searchedSale = value;
//            OnPropertyChanged(nameof(SearchedSale));
//        }
//    }

//    private bool _isGenerateSaleViewVisible = true;
//    public bool IsGenerateSaleViewVisible
//    {
//        get => _isGenerateSaleViewVisible;
//        set
//        {
//            _isGenerateSaleViewVisible = value;
//            OnPropertyChanged(nameof(IsGenerateSaleViewVisible));
//            OnPropertyChanged(nameof(IsAllSalesViewVisible));
//        }
//    }

//    public bool IsAllSalesViewVisible => !IsGenerateSaleViewVisible;

//    public ICommand ShowGenerateSaleViewCommand { get; }
//    public ICommand ShowAllSalesViewCommand { get; }
//    public ICommand LoadAllSalesCommand { get; }
//    public ICommand SearchSaleByIdCommand { get; }

//    public ICommand AddToCartCommand { get; }
//    public ICommand SubmitSaleCommand { get; }

//    public ICommand ViewSaleByIdCommand { get; }


//    public SalesViewModel()
//    {
//        AddToCartCommand = new RelayCommand(AddToCart);
//        SubmitSaleCommand = new RelayCommand(async () => await SubmitSale());
//        LoadAllSalesCommand = new RelayCommand(async () => await LoadAllSales());
//        SearchSaleByIdCommand = new RelayCommand(async () => await GetSaleById());

//        ShowGenerateSaleViewCommand = new RelayCommand(() => IsGenerateSaleViewVisible = true);
//        ShowAllSalesViewCommand = new RelayCommand(async () =>
//        {
//            IsGenerateSaleViewVisible = false;
//            await LoadAllSales();
//        });

//        ViewSaleByIdCommand = new RelayCommand<int>(async (saleId) => await ViewSaleById(saleId));

//        LoadProducts();
//    }

//    private async void LoadProducts()
//    {
//        var productList = await _productService.GetAllProductsAsync();
//        Products.Clear();
//        foreach (var product in productList)
//            Products.Add(product);
//    }

//    private void AddToCart()
//    {
//        if (SelectedProduct != null && Quantity > 0)
//        {
//            var unitPrice = SelectedProduct.Price;
//            var total = unitPrice * Quantity;

//            var itemForRequest = new SaleItemDto
//            {
//                ProductId = SelectedProduct.Id,
//                Quantity = Quantity
//            };
//            CartItems.Add(itemForRequest);

//            var itemForDisplay = new SaleItemDto
//            {
//                ProductId = SelectedProduct.Id,
//                Quantity = Quantity,
//                Name = SelectedProduct.Name,
//                UnitPrice = unitPrice,
//                Total = total
//            };
//            SaleItems.Add(itemForDisplay);
//            CalculateTotalAmount();
//            Quantity = 1;
//            SelectedProduct = null;
//        }
//    }

//    private void CalculateTotalAmount()
//    {
//        TotalAmount = SaleItems.Sum(item => item.Total);
//    }

//    private void CalculateTotalAmountDiscount()
//    {
//        var subtotal = SaleItems.Sum(item => item.Total);
//        var discountAmount = subtotal * (DiscountPercentage / 100);
//        TotalAmount = subtotal - discountAmount;
//    }

//    private async Task SubmitSale()
//    {
//        //var sale = new CreateSaleDto
//        //{
//        //    CustomerName = CustomerName,
//        //    Items = CartItems.ToList()
//        //};

//        var sale = new CreateSaleDto
//        {
//            CustomerName = CustomerName,
//            Items = CartItems.ToList(),
//            DiscountPercentage = OverallDiscountPercentage // Pass the overall discount
//        };


//        var result = await _saleService.CreateSaleAsync(sale);
//        if (result != null)
//        {
//           // MessageBox.Show($"Sale Created! ID: {result.Id}\nTotal: {result.Items.Sum(i => i.Total)}");

//            await DownloadInvoice(result.Id);

//            CartItems.Clear();
//            SaleItems.Clear();

//            CustomerName = string.Empty;
//            Quantity = 1;
//            SelectedProduct = null;
//            TotalAmount = 0;
//            CartItems.Clear();
//            SaleItems.Clear();
//            CustomerName = string.Empty;
//            Quantity = 1;
//            SelectedProduct = null;
//            TotalAmount = 0;
//            OverallDiscountPercentage = 0;
//            GrandTotalAmount = 0;
//        }
//        else
//        {
//            MessageBox.Show("Sale creation failed!");
//        }
//    }


//    private async Task DownloadInvoice(int saleId)
//    {
//        var invoiceUrl = $"https://localhost:7228/api/Sales/{saleId}/invoice";
//        var pdfResponse = await _saleService.GetInvoiceAsync(invoiceUrl);

//        if (pdfResponse != null)
//        {
//            var projectDirectory = Directory.GetCurrentDirectory(); 

//            var invoicesFolderPath = Path.Combine(projectDirectory, "Invoices");

//            if (!Directory.Exists(invoicesFolderPath))
//            {
//                Directory.CreateDirectory(invoicesFolderPath);
//            }

//            var filePath = Path.Combine(invoicesFolderPath, $"Invoice_{saleId}.pdf");

//            File.WriteAllBytes(filePath, pdfResponse);

//           // MessageBox.Show($"Invoice saved at: {filePath}");

//            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(filePath) { UseShellExecute = true });
//        }
//        else
//        {
//            MessageBox.Show("Failed to download invoice.");
//        }
//    }


//    private async Task LoadAllSales()
//    {
//        AllSales.Clear();
//        var sales = await _saleService.GetAllSalesAsync();
//        foreach (var sale in sales)
//            AllSales.Add(sale);
//    }

//    private async Task GetSaleById()
//    {
//        if (SearchSaleId > 0)
//        {
//            SearchedSale = await _saleService.GetSaleByIdAsync(SearchSaleId);
//        }
//    }

//    private async Task ViewSaleById(int saleId)
//    {
//        var sale = await _saleService.GetSaleByIdAsync(saleId);
//        if (sale != null)
//        {
//            Application.Current.Dispatcher.Invoke(() =>
//            {
//                var window = new SaleDetailsWindow(sale);
//                window.ShowDialog();
//            });
//        }
//        else
//        {
//            MessageBox.Show("Sale not found.");
//        }
//    }

//    protected void OnPropertyChanged(string propertyName)
//    {
//        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
//    }


//    public event PropertyChangedEventHandler PropertyChanged;
//}


using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows;
using WPF_Medical_Inventory_Managment_Systemm.Services;
using WPF_Medical_Inventory_Managment_Systemm.Models;

public class SalesViewModel : INotifyPropertyChanged
{
    private readonly ProductApiService _productService = new ProductApiService();
    private readonly SalesApiService _saleService = new SalesApiService();

    public ObservableCollection<Product> Products { get; set; } = new ObservableCollection<Product>();
    public ObservableCollection<SaleItemDto> CartItems { get; set; } = new ObservableCollection<SaleItemDto>();
    public ObservableCollection<SaleItemDto> SaleItems { get; set; } = new ObservableCollection<SaleItemDto>();
    private string _customerName;
    public string CustomerName
    {
        get => _customerName;
        set
        {
            _customerName = value;
            OnPropertyChanged(nameof(CustomerName));
        }
    }

    private Product _selectedProduct;
    public Product SelectedProduct
    {
        get => _selectedProduct;
        set
        {
            _selectedProduct = value;
            OnPropertyChanged(nameof(SelectedProduct));
        }
    }

    private int _quantity = 1;
    public int Quantity
    {
        get => _quantity;
        set
        {
            _quantity = value;
            OnPropertyChanged(nameof(Quantity));
        }
    }

    private decimal _totalAmount;
    public decimal TotalAmount
    {
        get => _totalAmount;
        set
        {
            _totalAmount = value;
            OnPropertyChanged(nameof(TotalAmount));
        }
    }


    public ICommand AddToCartCommand { get; }
    public ICommand SubmitSaleCommand { get; }

    public SalesViewModel()
    {
        AddToCartCommand = new RelayCommand(AddToCart);
        SubmitSaleCommand = new RelayCommand(async () => await SubmitSale());
        LoadProducts();
    }

    private async void LoadProducts()
    {
        var productList = await _productService.GetAllProductsAsync();
        Products.Clear();
        foreach (var product in productList)
            Products.Add(product);
    }

    private void AddToCart()
    {
        if (SelectedProduct != null && Quantity > 0)
        {
            var unitPrice = SelectedProduct.Price;
            var total = unitPrice * Quantity;

            var itemForRequest = new SaleItemDto
            {
                ProductId = SelectedProduct.Id,
                Quantity = Quantity
            };
            CartItems.Add(itemForRequest);

            var itemForDisplay = new SaleItemDto
            {
                ProductId = SelectedProduct.Id,
                Quantity = Quantity,
                Name = SelectedProduct.Name,
                UnitPrice = unitPrice,
                Total = total
            };
            SaleItems.Add(itemForDisplay);
            CalculateTotalAmount();
            Quantity = 1;
            SelectedProduct = null;
        }
    }

    private void CalculateTotalAmount()
    {
        TotalAmount = SaleItems.Sum(item => item.Total);
    }


    private async Task SubmitSale()
    {
        var sale = new CreateSaleDto
        {
            CustomerName = CustomerName,
            Items = CartItems.ToList()
        };

        var result = await _saleService.CreateSaleAsync(sale);
        if (result != null)
        {
            MessageBox.Show($"Sale Created! ID: {result.Id}\nTotal: {result.Items.Sum(i => i.Total)}");

            CartItems.Clear();
            SaleItems.Clear();

            CustomerName = string.Empty;
            Quantity = 1;
            SelectedProduct = null;
            TotalAmount = 0;
        }
        else
        {
            MessageBox.Show("Sale creation failed!");
        }
    }
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }


    public event PropertyChangedEventHandler PropertyChanged;
}


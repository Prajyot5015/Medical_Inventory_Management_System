using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using System.Windows.Controls;
using WPF_Medical_Inventory_Managment_Systemm.Services;
using WPF_Medical_Inventory_Managment_Systemm.ViewModel;
using WPF_Medical_Inventory_Managment_Systemm.ViewModels;
using WPF_Medical_Inventory_Managment_Systemm.Views;

namespace WPF_Medical_Inventory_Managment_Systemm
{
    public partial class MainWindow : MahApps.Metro.Controls.MetroWindow//Window
    {
        private readonly ServiceProvider _provider;

        public MainWindow()
        {
            InitializeComponent();

            var services = new ServiceCollection();
            ConfigureServices(services);
            _provider = services.BuildServiceProvider();

            NavigateToManufacturerPage(null, null);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient<ManufacturersApiService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7228/api/");
            });

            services.AddHttpClient<BrandService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7228/api/");
            });

            services.AddHttpClient<ProductApiService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7228/api/");
            });

            services.AddTransient<ManufacturersViewModel>();
            services.AddTransient<BrandViewModel>();
            services.AddTransient<ProductViewModel>();

            services.AddTransient<ManufacturersPage>();
            services.AddTransient<BrandView>();
            services.AddTransient<ProductWindow>();
        }

        private void NavigateToManufacturerPage(object sender, RoutedEventArgs e)
        {
            var page = _provider.GetRequiredService<ManufacturersPage>();
            page.DataContext = _provider.GetRequiredService<ManufacturersViewModel>();
            MainFrame.Navigate(page);
        }

        private void NavigateToBrandPage(object sender, RoutedEventArgs e)
        {
            var page = _provider.GetRequiredService<BrandView>();
            page.DataContext = _provider.GetRequiredService<BrandViewModel>();
            MainFrame.Navigate(page);
        }

        private void NavigateToProductPage(object sender, RoutedEventArgs e)
        {
            var page = _provider.GetRequiredService<ProductWindow>();
            page.DataContext = _provider.GetRequiredService<ProductViewModel>();
            MainFrame.Navigate(page);
        }
    }
}



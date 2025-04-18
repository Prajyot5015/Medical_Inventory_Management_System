﻿using Microsoft.Extensions.DependencyInjection;
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

            services.AddHttpClient<StockApiService>(client =>
            {
                client.BaseAddress = new Uri("http://localhost:5000/api/stock"); // Update to your stock API URL
            });

            services.AddTransient<ManufacturersViewModel>();
            services.AddTransient<BrandViewModel>();
            services.AddTransient<ProductViewModel>();
            services.AddTransient<StockViewModel>();

            services.AddTransient<ManufacturersPage>();
            services.AddTransient<BrandView>();
            services.AddTransient<ProductWindow>();
            services.AddTransient<StockView>();
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
        private void NavigateToStockPage(object sender, RoutedEventArgs e)
        {
            var page = _provider.GetRequiredService<StockView>();
            page.DataContext = _provider.GetRequiredService<StockViewModel>();  // Bind the StockViewModel to the view
            MainFrame.Navigate(page);
        }

    }
}



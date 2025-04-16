using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Extensions.DependencyInjection;
using System;
using WPF_Medical_Inventory_Managment_Systemm.Services;
using WPF_Medical_Inventory_Managment_Systemm.ViewModels;
using WPF_Medical_Inventory_Managment_Systemm.Views;

namespace WPF_Medical_Inventory_Managment_Systemm
{
    public partial class MainWindow : Window
    {
        private readonly ServiceProvider _provider;

        public MainWindow()
        {
            InitializeComponent();

            var services = new ServiceCollection();
            ConfigureServices(services);
            _provider = services.BuildServiceProvider();

            NavigateToManufacturerPage();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient<ManufacturersApiService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7228/api/");
            });


            services.AddTransient<ManufacturersViewModel>();
            services.AddTransient<ManufacturersPage>();
        }

        private void NavigateToManufacturerPage()
        {
            var page = _provider.GetRequiredService<ManufacturersPage>();
            var viewModel = _provider.GetRequiredService<ManufacturersViewModel>();
            page.DataContext = viewModel;
            MainFrame.Navigate(page);
        }
    }
}

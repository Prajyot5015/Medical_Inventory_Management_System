using System;
using System.Net.Http;
using System.Windows;

namespace WPF_Medical_Inventory_Managment_Systemm
{
    public partial class App : Application
    {
        public static HttpClient HttpClient { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            HttpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5000/api/") // 🔁 Adjust this if needed
            };
        }
    }
}

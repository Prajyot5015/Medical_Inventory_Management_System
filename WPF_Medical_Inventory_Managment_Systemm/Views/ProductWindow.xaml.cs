using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPF_Medical_Inventory_Managment_Systemm.ViewModel;

namespace WPF_Medical_Inventory_Managment_Systemm.Views
{
    /// <summary>
    /// Interaction logic for ProductWindow.xaml
    /// </summary>
    public partial class ProductWindow : Page
    {
        public ProductWindow()
        {
            InitializeComponent();
            //Loaded += async (s, e) =>
            //{
            //    if (DataContext is ProductViewModel vm)
            //    {
            //        await vm.LoadCommand.Execute(null);
            //    }
            //};
            Loaded += (s, e) =>
            {
                if (DataContext is ProductViewModel vm)
                {
                    vm.LoadCommand.Execute(null); // ✅ No await
                }
            };

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}

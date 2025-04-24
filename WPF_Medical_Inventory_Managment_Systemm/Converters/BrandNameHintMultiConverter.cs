using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WPF_Medical_Inventory_Managment_Systemm.Converters
{
    public class BrandNameHintMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string brandName = values[0] as string;
            bool isManuallyEntered = values.Length > 1 && values[1] is bool b && b;

            if (string.IsNullOrWhiteSpace(brandName))
                return "Enter brand name";

            return isManuallyEntered ? "Entered Brand Name" : "Selected Brand Name";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}

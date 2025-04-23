using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WPF_Medical_Inventory_Managment_Systemm.Converters
{
    public class DecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString() ?? string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrWhiteSpace(value as string))
                return 0m;

            if (decimal.TryParse(value.ToString(), NumberStyles.Any, culture, out decimal result))
                return result;

            return 0m; // Fallback to 0 if parsing fails
        }
    }
}

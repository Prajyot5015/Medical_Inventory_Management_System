using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace WPF_Medical_Inventory_Managment_Systemm.Converters
{
    public class BoolToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool boolValue = (bool)value;
            bool param = System.Convert.ToBoolean(parameter);

            return boolValue == param ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F7C82")) : Brushes.Transparent;

            // return boolValue == param ? Brushes.SteelBlue : Brushes.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Globalization;
using System.Windows.Data;

namespace WPF_Medical_Inventory_Managment_Systemm.Converters
{
    public class QuantityConverter : IValueConverter
    {
        private string _lastValidInput = "0"; // Track last valid value

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Show empty string when value is 0 (allows complete deletion)
            return (value is decimal d && d == 0) ? string.Empty : value?.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string input = value?.ToString();

            // Case 1: Empty string → return 0 (but won't force UI update)
            if (string.IsNullOrEmpty(input))
            {
                _lastValidInput = "0";
                return Binding.DoNothing; // Prevents forcing 0 back into UI
            }

            // Case 2: Non-numeric → keep last valid input
            if (!decimal.TryParse(input, out decimal result))
            {
                return decimal.Parse(_lastValidInput);
            }

            // Case 3: Negative → return 0
            if (result < 0)
            {
                _lastValidInput = "0";
                return 0m;
            }

            // Valid positive number
            _lastValidInput = input;
            return result;
        }
    }
}

//using System;
//using System.Globalization;
//using System.Windows.Data;

//namespace WPF_Medical_Inventory_Managment_Systemm.Converters
//{
//    public class QuantityConverter : IValueConverter
//    {
//        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            // Display the value as-is (decimal -> string)
//            return value?.ToString();
//        }

//        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            string input = value?.ToString();

//            // Case 1: Empty or invalid input → return 0
//            if (string.IsNullOrWhiteSpace(input))
//                return 0m;

//            // Case 2: Non-numeric input → return 0
//            if (!decimal.TryParse(input, out decimal result))
//                return 0m;

//            // Case 3: Negative input → return 0
//            if (result < 0)
//                return 0m;

//            // Valid positive number → return the parsed value
//            return result;
//        }
//    }
//}


//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Data;

//namespace WPF_Medical_Inventory_Managment_Systemm.Converters
//{
//    public class QuantityConverter : IValueConverter
//    {
//        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            return value?.ToString() ?? "0";
//        }

//        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            return decimal.TryParse(value?.ToString(), out decimal result) ? result : 0m;
//        }
//    }
//}

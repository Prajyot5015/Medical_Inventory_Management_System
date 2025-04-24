using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WPF_Medical_Inventory_Managment_Systemm.Validation
{
    public class CustomerNameValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string customerName = value as string;
            if (string.IsNullOrWhiteSpace(customerName))
            {
                return new ValidationResult(false, "Customer Name is required.");
            }
            return ValidationResult.ValidResult;
        }
    }


}

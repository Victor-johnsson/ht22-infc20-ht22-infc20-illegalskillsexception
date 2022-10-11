using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ProjAssignment
{
    public class NameValidator : ValidationRule
    {
        public string Name { get; set; }
        public string Location { get; set; }

        public override ValidationResult Validate
          (object value, CultureInfo cultureInfo)
        {
            if (value == null || value.ToString().IsNullOrEmpty())
                return new ValidationResult(false, "value cannot be empty.");
            else
            {
                if (value.ToString().Length > 255)
                    return new ValidationResult(false, "value cannot be more than 255 characters long.");
            }
            return ValidationResult.ValidResult;
        }
    }
}

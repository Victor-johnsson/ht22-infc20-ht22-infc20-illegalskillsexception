using Azure;
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
        public string PhoneNbr { get; set; }
        public string Species { get; set; }

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

    public class IntValidator : ValidationRule
    {
        public int Min { get; set; }    
        public int Max { get; set; }    
        public int Size { get; set;}
        public int Amount { get; set; }
        public int Capacity { get; set; }

        public IntValidator()
        {

        }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int integer = 0;


            try
            {
                if (((string)value).Length > 0)
                    integer = Int32.Parse((String)value);
            }
            catch (Exception e)
            {
                return new ValidationResult(false, $"Illegal characters or {e.Message}");
            }

            if ((integer < Min) || (integer > Max))
            {
                return new ValidationResult(false,
                  $"Please enter an value in the range: {Min}-{Max}.");
            }
            return ValidationResult.ValidResult;

        }
    }
}

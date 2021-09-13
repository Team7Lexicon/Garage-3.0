using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Garage3._0.Models.ViewModels;

namespace Garage3._0.Validation
{
    public class CheckPassWordAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            const string errorMessage = "Password must have at least 8 characters, have upercase and numbers";

            if (value is string input)
            {                
                if (string.IsNullOrWhiteSpace(input))
                {                   
                    return new ValidationResult(errorMessage);
                }

                Regex ValidPasswordRegex = CreateValidPasswordRegex();

                Regex CreateValidPasswordRegex()
                {
                    string validEmailPattern = @"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=).*$";

                    return new Regex(validEmailPattern, RegexOptions.IgnoreCase);
                }

                if (ValidPasswordRegex.IsMatch(input))
                {
                    return ValidationResult.Success;
                }
                else
                    return new ValidationResult(errorMessage);
            }
            return new ValidationResult(errorMessage);
        }
    }

}

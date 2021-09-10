using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using static AutoMapper.Internal.ExpressionFactory;
using Garage3._0.Models.ViewModels;

namespace Garage3._0.Validation
{
    public class CheckEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            const string errorMessage = "Invalid Email Address";

            if (value is string input)
            {              
                //Check if input is null
                 if (string.IsNullOrWhiteSpace(input))
                 {
                     return new ValidationResult(errorMessage);
                 }

                 Regex ValidEmailRegex = CreateValidEmailRegex();
                  
                //Create a regex to validate email   
                 Regex CreateValidEmailRegex()
                 {
                     string validEmailPattern = @"^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$";

                     return new Regex(validEmailPattern, RegexOptions.IgnoreCase);
                 }

                //if the email is ok then successs
                 if (ValidEmailRegex.IsMatch(input))
                 {
                     return ValidationResult.Success;
                 }
            }
            return new ValidationResult(errorMessage);
        }

    }
}

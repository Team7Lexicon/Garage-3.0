using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Garage3._0.Validation
{
    public class CheckEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            const string errorMessage = "Invalid Email Address";

            if (value is string input)
            {
                 /*var model = (MemberViewModel)validationContext.ObjectInstance;//Not created yet

                 if (string.IsNullOrWhiteSpace(model.Email))//input??
                 {
                     return new ValidationResult(errorMessage);
                 }

                 Regex ValidEmailRegex = CreateValidEmailRegex();

                 Regex CreateValidEmailRegex()
                 {
                     string validEmailPattern = @"^[a - z0 - 9!#$%&'*+\/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+\/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$";

                     return new Regex(validEmailPattern, RegexOptions.IgnoreCase);
                 }

                 if (ValidEmailRegex.IsMatch(model.Email))
                 {
                     return ValidationResult.Success;
                 }*/
            }
            return new ValidationResult(errorMessage);
        }

    }
}

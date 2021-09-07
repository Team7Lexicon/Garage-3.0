using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Garage3._0.Validation
{
    public class CheckEmailAtribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            const string errorMessage = "Invalid Email Address";

            if (value is string input)
            {
                /*var model = (MemberViewModel)validationContext.ObjectInstance;//Not created yet

                try
                {
                    MailAddress m = new MailAddress(model.Email);
                    return ValidationResult.Success;
                }
                catch (FormatException)
                {
                    return new ValidationResult(errorMessage);
                }  */                  
            }
            return new ValidationResult(errorMessage);
        }

    }
}

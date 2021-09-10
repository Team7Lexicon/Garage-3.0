using Garage3._0.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Garage3._0.Models.ViewModels;


namespace Garage3._0.Validation
{
    public class CheckFirstAndLastNamesAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            const string errorMessage = "First name cannot be equal to last name";

            if (value is string input)
            {
                var model = (Member)validationContext.ObjectInstance;
                if (model.FirstName == model.LastName)
                    return new ValidationResult(errorMessage);
                else
                    return ValidationResult.Success;
            }
            return new ValidationResult(errorMessage);
        }
    }
    
}

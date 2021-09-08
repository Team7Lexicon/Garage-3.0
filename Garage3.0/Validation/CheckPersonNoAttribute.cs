using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3._0.Validation
{
    public class CheckPersonNoAttribute : ValidationAttribute
    {
        private readonly int maxLength;

        public CheckPersonNoAttribute(int maxLength)
        {
            ErrorMessageResourceName = "PersonNo must be 12 characters long and only numbers are allowed";
            this.maxLength = maxLength;
        }

        public override bool IsValid(object value)
        {
            if (value is string input)
            {
                foreach (char c in input)//Check if only numbers are in iput
                {
                    if ((c < '0' || c > '9') && (input.Length == maxLength))
                        return false;
                }
            }
            return false;
        }
    }

}

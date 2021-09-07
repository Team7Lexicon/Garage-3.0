using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3._0.Validation
{
    public class CheckPersonNoAtribute : ValidationAttribute
    {
        private readonly int maxLength;

        public CheckPersonNoAtribute(int maxLength)
        {
            ErrorMessageResourceName = "PersonNo must be 12 ccharacters long";
            this.maxLength = maxLength;
        }

        public override bool IsValid(object value)
        {
            if (value is string input)
            {
                foreach (char c in input)//Check if only numbers are in iput
                {
                    if (c < '0' || c > '9')
                        return false;
                }

                if (input.Length == maxLength)
                    return true;
            }
            return false;
        }
    }

}

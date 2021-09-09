﻿/* using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Garage3._0.Validation
{
    public class CheckRegistrationNoAttribute : ValidationAttribute
    {
        private readonly int maxLength;

        public CheckRegistrationNoAttribute(int maxLength)
        {
            ErrorMessageResourceName = "PersonNo must be 12 characters long and only numbers are allowed";
            this.maxLength = maxLength;
        }

        public override bool IsValid(object value)
        {
            if (value is string input)
            {
                string strRegex = "^[0 - 9][A - Z]$";

                Regex rx = new Regex(strRegex);
                if(rx.IsMatch(input.RegNo) && (input.Length == maxLength))
                    return true;
            }
            return false;
        }
    }
}
*/
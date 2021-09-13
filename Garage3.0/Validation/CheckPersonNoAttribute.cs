using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;


namespace Garage3._0.Validation
{
    public class CheckPersonNoAttribute : ValidationAttribute
    {
        private readonly int maxLength;

        public CheckPersonNoAttribute(int maxLength)
        {            
            ErrorMessage  = "PersonNo must be 12 characters long and only numbers are allowed and you must be at least 18 years old";
            this.maxLength = maxLength;
        }
        
        public override bool IsValid(object value)
        {
            if (value is string input)
            {
                //Remove blanc spaces and dash characters
                input = input.Replace(" ", "").Replace("-", "");

                if (input.Length != maxLength) return false;

                foreach (char c in input)//Check if only numbers are in input
                {
                    if ((c < '0' || c > '9') && (input.Length == maxLength))
                        return false;
                }

                //if the members age is < 18 then we return false 
                if (!CheckAge(input))
                {
                    return false;
                }

            }
            return true;
        }
        bool CheckAge(string personNo)
        {
            //Convert persNumbert to DateTime
            string year = personNo.Substring(0, 4);
            string month = personNo.Substring(4, 2);
            string day = personNo.Substring(6, 2);
            string personNoDate = $"{day}/{month}/{year}";

            CultureInfo provider = CultureInfo.InvariantCulture;
            DateTime birthDate = DateTime.ParseExact(personNoDate, "dd/mm/yyyy", provider);

            DateTime actualTime = DateTime.Now;

            //Check age, if < than 18 return "false"
            TimeSpan tmpTime = actualTime.Subtract(birthDate);

            if ((tmpTime.TotalDays / 365) <= 17)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}

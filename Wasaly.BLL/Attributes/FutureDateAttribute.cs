using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasaly.BLL.Attributes
{
    public class FutureDateAttribute:ValidationAttribute
    {
        public string ErrorMessage { get; set; } 
        public override bool IsValid(object value)
        {
            if (value == null)
                return true;

            DateTime date = Convert.ToDateTime(value);

            return date.Date >= DateTime.Today;
        }

        public override string FormatErrorMessage(string message)
        {
            return ErrorMessage;
        }
    }
}

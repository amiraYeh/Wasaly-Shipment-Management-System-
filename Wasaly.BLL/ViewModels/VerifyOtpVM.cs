using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasaly.BLL.ViewModels
{
    public class VerifyOtpVM
    {
        public int ShipmentId { get; set; }

        [Required(ErrorMessage = "الكود مطلوب")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "الكود لازم يكون 6 أرقام")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "الكود لازم يكون أرقام فقط")]
        public string OtpCode { get; set; }
    }
}

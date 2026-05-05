using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasaly.BLL.ViewModels
{
    public class MerchantCreateViewModel
    {
        [Required(ErrorMessage = "Store name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Store name must be between 3 and 100 characters")]
        public string StoreName { get; set; } = null!;

      
        [Required(ErrorMessage = "Business type is required")]
        [StringLength(50)]
        public string BusinessType { get; set; } = null!;
    }
}

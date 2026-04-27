using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasaly.BLL.ViewModels
{
    internal class CourierCreateViewModel
    {
        [Required(ErrorMessage = "Store name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Store name must be between 3 and 100 characters")]
        public string StoreName { get; set; } = null!;

        [Required(ErrorMessage = "Store address is required")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Address must be between 5 and 200 characters")]
        public string StoreAddress { get; set; } = null!;

        [Required(ErrorMessage = "Business type is required")]
        [StringLength(50)]
        public string BusinessType { get; set; } = null!;
    }
}

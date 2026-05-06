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
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "يجب أن يكون اسم المتجر بين 3 و 100 حرفًا")]
        public string StoreName { get; set; } = null!;


        [StringLength(50, ErrorMessage = "يجب ألا يزيد {0} عن {1} حرفًا")]
        public string BusinessType { get; set; } = null!;
    }
}

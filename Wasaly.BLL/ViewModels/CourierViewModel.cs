using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasaly.BLL.ViewModels
{
    public class CourierViewModel
    {
        [Required(ErrorMessage = "هذا الحقل مطلوب")]

        public IFormFile NationalIdImage { get; set; } = null!;

        [Required(ErrorMessage = "هذا الحقل مطلوب")]



  
        public IFormFile DrivingLicenseImage { get; set; } = null!;

        [Required(ErrorMessage = "هذا الحقل مطلوب")]


        public IFormFile ProfileImage { get; set; } = null!;
    }
}

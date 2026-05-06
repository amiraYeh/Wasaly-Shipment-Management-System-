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
<<<<<<< HEAD
        [Required(ErrorMessage = "هذا الحقل مطلوب")]

        public IFormFile NationalIdImage { get; set; } = null!;

        [Required(ErrorMessage = "هذا الحقل مطلوب")]
=======
        [Required]
        //[FileExtensions(Extensions = "jpg,jpeg,png", ErrorMessage = "Only JPG, JPEG, PNG files are allowed")]

        public IFormFile NationalIdImage { get; set; } = null!;

        [Required]
        //[FileExtensions(Extensions = "jpg,jpeg,png", ErrorMessage = "Only JPG, JPEG, PNG files are allowed")]
>>>>>>> 208e08be0b5fdb49ff2831be02c0b692591ecaf3

        public IFormFile DrivingLicenseImage { get; set; } = null!;
        [Required(ErrorMessage = "هذا الحقل مطلوب")]

<<<<<<< HEAD
=======
        [Required]
        //[FileExtensions(Extensions = "jpg,jpeg,png", ErrorMessage = "Only JPG, JPEG, PNG files are allowed")]
>>>>>>> 208e08be0b5fdb49ff2831be02c0b692591ecaf3
        public IFormFile ProfileImage { get; set; } = null!;
    }
}

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
        [Required]
        //[FileExtensions(Extensions = "jpg,jpeg,png", ErrorMessage = "Only JPG, JPEG, PNG files are allowed")]

        public IFormFile NationalIdImage { get; set; } = null!;

        [Required]
        //[FileExtensions(Extensions = "jpg,jpeg,png", ErrorMessage = "Only JPG, JPEG, PNG files are allowed")]

        public IFormFile DrivingLicenseImage { get; set; } = null!;

        [Required]
        //[FileExtensions(Extensions = "jpg,jpeg,png", ErrorMessage = "Only JPG, JPEG, PNG files are allowed")]
        public IFormFile ProfileImage { get; set; } = null!;
    }
}

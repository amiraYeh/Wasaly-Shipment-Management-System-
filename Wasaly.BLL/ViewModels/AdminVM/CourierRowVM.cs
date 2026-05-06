using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasaly.BLL.ViewModels.AdminVM
{
    public class CourierRowVM
    {
        
        public string Id { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Region { get; set; } = null!;
        public bool IsVerified { get; set; }
        public decimal Rating { get; set; }

        //  Details
        public string Email { get; set; } = null!;
        public string NationalIdImagePath { get; set; } = null!;
        public string DrivingLicenseImagePath { get; set; } = null!;
        public string ProfileImagePath { get; set; } = null!;

        // Helpers
        public string StatusText => IsVerified ? "موثق" : "في الانتظار";
        public string StatusClass => IsVerified ? "bg-success" : "bg-warning";
    }
}

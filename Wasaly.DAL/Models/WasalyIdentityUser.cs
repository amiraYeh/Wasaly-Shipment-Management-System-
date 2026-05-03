using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasaly.DAL.Models
{
    public enum Gender
    {
        Male,
        Female
    }

    public class WasalyIdentityUser: IdentityUser
    {
        public int Id { get; set; }

        public string FullName { get; set; } = null!;
        public int LocationId { get; set; }
        public Location Location { get; set; } = null!;

        public Gender Gender { get; set; }

        public int Age { get; set; }
        public string Region { get; set; }
        public string PhoneNumber { get; set; } = null!;

    }
}

using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasaly.DAL.Enums;

namespace Wasaly.DAL.Models
{

    public class WasalyIdentityUser: IdentityUser
    {

        public string FullName { get; set; } = null!;
        public int LocationId { get; set; }
        public Location Location { get; set; } = null!;
        public int? Rating { get; set; }
        public Gender gender { get; set; }

        public int Age { get; set; }

        public region Region { get; set; }

        public string PhoneNumber { get; set; } = null!;

    }
}

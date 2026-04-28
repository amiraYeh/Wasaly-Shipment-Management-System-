using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasaly.DAL.Models
{
    public class Notification
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public bool IsRead { get; set; } = false;

        public DateTime CreatedAt { get; set; }

        public string WasalyIdentityUserId { get; set; } = null!;

        public virtual WasalyIdentityUser WasalyIdentityUser { get; set; } = null!;
    }
}

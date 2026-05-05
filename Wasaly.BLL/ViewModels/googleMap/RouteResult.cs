using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasaly.BLL.ViewModels.googleMap
{
    public class RouteResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public string Distance { get; set; }      // "18.3 km"
        public string Duration { get; set; }      // "25 mins"
        public double DistanceKm { get; set; }    // 18.3
    }
}

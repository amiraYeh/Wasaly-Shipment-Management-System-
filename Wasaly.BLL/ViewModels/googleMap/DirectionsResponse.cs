using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Wasaly.BLL.ViewModels.googleMap
{
    public class DirectionsResponse
    {
        //[JsonPropertyName("status")]
        public string Status { get; set; }

        //[JsonPropertyName("routes")]
        public List<Route> Routes { get; set; }
    }
}


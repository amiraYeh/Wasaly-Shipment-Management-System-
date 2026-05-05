using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Wasaly.BLL.ViewModels.googleMap
{
    public class Route
    {
        //[JsonPropertyName("legs")]
        public List<Leg> Legs { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Text.Json;

namespace Wasaly.BLL.ViewModels.googleMap
{
    public class Leg
    {
        //[JsonPropertyName("distance")]
        public ValueText Distance { get; set; }

        //[JsonPropertyName("duration")]
        public ValueText Duration { get; set; }
    }
}

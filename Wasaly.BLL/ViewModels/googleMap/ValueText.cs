using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Wasaly.BLL.ViewModels.googleMap
{
    public class ValueText
    {
        //[JsonPropertyName("value")]
        public int Value { get; set; }

        //[JsonPropertyName("text")]
        public string Text { get; set; }
        
    }
}

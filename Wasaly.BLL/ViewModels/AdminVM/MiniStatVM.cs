using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasaly.BLL.ViewModels.AdminVM
{
    public class MiniStatVM
    {
        public string Icon { get; set; } = "";
        public string Label { get; set; } = "";
        public int Value { get; set; }
        public string Color { get; set; } = "primary";
    }
}

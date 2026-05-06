using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasaly.BLL.ViewModels.AdminVM
{
    public class FilterCardVM
    {
        public string? SearchTerm { get; set; }
        public string? StatusFilter { get; set; }
        public string SearchPlaceholder { get; set; } = "ابحث...";
        public string Action { get; set; } = "";
        public List<string> StatusOptions { get; set; } = new();
    }
}

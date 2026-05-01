using Microsoft.AspNetCore.Mvc;
using Wasaly.BLL.ViewModels;
using Wasaly.DAL.Repositories;

namespace Wasaly.PL.Controllers
{
    public class ShipmentController : Controller
    {

        public ShipmentController()
        {
 
        }
        public async Task<IActionResult> Index()
        {
           
            return View();
        }
    }
}

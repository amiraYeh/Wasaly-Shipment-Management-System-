using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasaly.BLL.ViewModels.googleMap;

namespace Wasaly.BLL.Services
{
    public interface IGoogleMapService
    {
        string GetApi();
         //Task<RouteResult> getRouteAsync(double originLat, double originLng,double destnationLat, double destnationLng);
         Task<double> getKmDistanceAsync(double originLat, double originLng,double destnationLat, double destnationLng);


    }
}

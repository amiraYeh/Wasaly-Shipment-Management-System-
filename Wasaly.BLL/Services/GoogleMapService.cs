using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Wasaly.BLL.ViewModels.googleMap;

namespace Wasaly.BLL.Services
{
    public class GoogleMapService : IGoogleMapService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public GoogleMapService(IConfiguration configuration)
        {
           _configuration = configuration;
            _httpClient = new HttpClient();
            _apiKey = _configuration["googleMaps:Key"];
        }
        public string GetApi()
        {   
            return _apiKey;
        }

        public async Task<double> getKmDistanceAsync(double originLat, double originLng, double destnationLat, double destnationLng)
        {
            var Url = getURL($"{originLat},{originLng}", $"{destnationLat},{destnationLng}");

            var res = await _httpClient.GetFromJsonAsync<DirectionsResponse>(Url);

            if (res?.Status != "OK") return 0;

            var distanceMeter = res.Routes[0].Legs[0].Distance.Value;

            return Math.Round(distanceMeter/1000.0, 2);
        }
        private string getURL(string origin,string destination)
        {
            return $"https://maps.googleapis.com/maps/api/directions/json" +
                    $"?origin={Uri.EscapeDataString(origin)}" +
                    $"&destination={Uri.EscapeDataString(destination)}" + 
                    $"&mode=driving" + $"&key={_apiKey}";

        }
    }
}

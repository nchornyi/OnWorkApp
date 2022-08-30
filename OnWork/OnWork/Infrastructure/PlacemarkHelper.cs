using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms.GoogleMaps;

namespace OnWork.Infrastructure
{
    public static class PlacemarkHelper
    {
        public static Placemark GetPlacemarkAsync(Position position)
        {
            var placemarks = Task.Run(() => Geocoding.GetPlacemarksAsync(position.Latitude, position.Longitude));
            placemarks.Wait();
            var place = placemarks.Result.FirstOrDefault();

            return place;
        }
    }
}

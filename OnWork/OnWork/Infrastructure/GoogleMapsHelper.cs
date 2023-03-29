using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Plugin.Geolocator;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace OnWork.Infrastructure
{
    public class GoogleMapsHelper
    {
        public static Position GetMyPosition()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;

            var location = Task.Run(async () => await locator.GetPositionAsync(TimeSpan.FromTicks(10000))).Result;
            var position = new Position(location.Latitude, location.Longitude);

            return position;
        }
    }
}

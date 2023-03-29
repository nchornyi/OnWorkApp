namespace OnWork.Models
{
    public class UserSettings
    {
        public UserSettings() 
        {
            this.MapType = Xamarin.Forms.GoogleMaps.MapType.Satellite.ToString();
        }
        public string MapType { get; set; }
    }
}

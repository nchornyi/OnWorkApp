using OnWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OnWork.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageSettings : ContentPage
    {
        private EUserType UserType;
        public event EventHandler<object> CallbackEvent;
        public List<string> MapTypeList = new List<string>();
        public PageSettings(string title, EUserType userType)
        {
            InitializeComponent();
            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.Black;
            UserType = userType;
            Title = title + userType.ToString();

            MapTypeList = Enum.GetNames(typeof(Xamarin.Forms.GoogleMaps.MapType)).ToList();
            MapTypePicker.ItemsSource = MapTypeList;
            MapTypePicker.SelectedItem = FirebaseHelper.CurrentUser?.Settings.MapType;
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            CallbackEvent?.Invoke(this, false);
        }

        private void MapTypePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var value = MapTypeList[MapTypePicker.SelectedIndex];

                var settings = new UserSettings() { MapType = value };

                FirebaseHelper.UpdateUserSettings(FirebaseHelper.CurrentUser.UserName, settings);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
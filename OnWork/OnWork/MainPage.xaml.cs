using OnWork.Models;
using Plugin.Geolocator;
using Rg.Plugins.Popup.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
namespace OnWork
{
    public partial class MainPage : ContentPage
    {

        private Color NavBackColor = Color.LightSlateGray;
        private Color BGDark;
        private Color BGLight;
        public EUserType UserType;
        private Distance distance = Distance.FromMeters(400);
        [Obsolete]
        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            LoadResources();
            btnEmployer_Pressed(this, null);

            LoadSettings();

            #region footer
            DefaultBackground();
            imgProfile.Source = ImageSource.FromResource("OnWork.Images.user.png");
            imgTasks.Source = ImageSource.FromResource("OnWork.Images.clipboard.png");
            imgRequests.Source = ImageSource.FromResource("OnWork.Images.request.png");
            imgSettings.Source = ImageSource.FromResource("OnWork.Images.settings.png");
            imgLogout.Source = ImageSource.FromResource("OnWork.Images.logout.png");
            //Tap Gesture Recognizer  
            var homeTap = new TapGestureRecognizer();
            homeTap.Tapped += (sender, e) =>
            {
                stckHome.BackgroundColor = NavBackColor;
            };
            stckHome.GestureRecognizers.Add(homeTap);
            var alarmTap = new TapGestureRecognizer();
            alarmTap.Tapped += (sender, e) =>
            {
                stckAlarm.BackgroundColor = NavBackColor;
            };
            stckAlarm.GestureRecognizers.Add(alarmTap);
            var cameraTap = new TapGestureRecognizer();
            cameraTap.Tapped += (sender, e) =>
            {
                stckCamera.BackgroundColor = NavBackColor;
            };
            stckCamera.GestureRecognizers.Add(cameraTap);
            var settingsTap = new TapGestureRecognizer();
            settingsTap.Tapped += (sender, e) =>
            {
                stckSettings.BackgroundColor = NavBackColor;
            };
            stckSettings.GestureRecognizers.Add(settingsTap);
            var logoutTap = new TapGestureRecognizer();
            logoutTap.Tapped += (sender, e) =>
            {
                stckLogout.BackgroundColor = NavBackColor;
            };
            stckLogout.GestureRecognizers.Add(logoutTap);
            #endregion

            Task.Run(() => LoadMapOnMyLocation());
            LoadPins();


        }

        private void LoadSettings()
        {
            var settings = FirebaseHelper.CurrentUser?.Settings;
            Enum.TryParse(settings.MapType, out MapType type);
            map.MapType =  settings.MapType == null ? MapType.Satellite : type;
        }

        private async Task LoadMapOnMyLocation()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;

            var location = await locator.GetPositionAsync(TimeSpan.FromTicks(10000));
            Position position = new Position(location.Latitude, location.Longitude);

            map.MoveToRegion(MapSpan.FromCenterAndRadius(position, distance));
        }

        [Obsolete]
        private Pin CreatePin(Position location, string title, string desc, string taskid)
        {
            var pin = new Pin()
            {
                Address = desc,
                IsVisible = true,
                Label = title,
                Tag = taskid,
                Icon = BitmapDescriptorFactory.FromBundle("base"),
                Position = location,
                Type = PinType.Place
            };
            pin.Clicked += Pin_Clicked;

            return pin;
        }

        [Obsolete]
        private void LoadPins()
        {
            var Tasks = Task.Run(async () => await FirebaseHelper.GetTasks()).Result;

            foreach (var item in Tasks)
            {
                map.Pins.Add(CreatePin(
                    new Position(item.TaskLocationItem.Location.Latitude,item.TaskLocationItem.Location.Longitude), 
                    item.Title + " " + item.Price,
                    item.Desc+ "(Click for additional info)", 
                    item.id));
            }

            if(map.Pins.Count != 0)
                map.MoveToRegion(MapSpan.FromCenterAndRadius(map.Pins.Last().Position, distance));
        }

        [Obsolete]
        private void ReloadPins()
        {
            map.Pins.Clear();
            LoadPins();
        }

        [Obsolete]
        private async void Pin_Clicked(object sender, EventArgs e)
        {
            var taskId = ((Pin)sender).Tag.ToString();
            var task = await FirebaseHelper.GetTaskItem(taskId);

            if (task != null)
            {
                var page = new Pages.Popup.PopupPageTask(task, UserType);
                page.CallbackEvent += PopupPageTaskClosed_CallbackEvent;
                await PopupNavigation.PushAsync(page);
            }
        }

        [Obsolete]
        private void PopupPageTaskClosed_CallbackEvent(object sender, object e)
        {
            DefaultBackground();
            ReloadPins();
        }

        public void DefaultBackground()
        {
            stckHome.BackgroundColor = BGLight;
            stckAlarm.BackgroundColor = BGLight;
            stckCamera.BackgroundColor = BGLight;
            stckLogout.BackgroundColor = BGLight;
            stckSettings.BackgroundColor = BGLight;
        }
        private void LoadResources()
        {
            try
            {
                BGDark = (Color)Application.Current.Resources["BGDark"];
                BGLight = (Color)Application.Current.Resources["BGLight"];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void map_MyLocationButtonClicked(object sender, MyLocationButtonClickedEventArgs e)
        {

        }



        private void btnEmployee_Pressed(object sender, System.EventArgs e)
        {
            btnEmployee.BorderWidth = 2;
            btnEmployer.BorderWidth = 0;

            btnEmployee.BackgroundColor = BGDark;
            btnEmployer.BackgroundColor = BGLight;

            UserType = EUserType.Employee;

        }

        private void btnEmployer_Pressed(object sender, System.EventArgs e)
        {
            btnEmployer.BorderWidth = 2;
            btnEmployee.BorderWidth = 0;

            btnEmployer.BackgroundColor = BGDark;//Color.Red;
            btnEmployee.BackgroundColor = BGLight;

            UserType = EUserType.Employer;
        }

        #region OnTapped
        private async void Profile_OnTapped(object sender, System.EventArgs e)
        {
            var page = new Pages.PageProfile("Profile - ", UserType);
            page.CallbackEvent += PopupPageTaskClosed_CallbackEvent;
            await Navigation.PushAsync(page);
        }
        [Obsolete]
        private async void Tasks_OnTapped(object sender, System.EventArgs e)
        {
            var page = new Pages.PageTasks("Tasks - ", UserType);
            page.CallbackEvent += PopupPageTaskClosed_CallbackEvent;
            await Navigation.PushAsync(page);
        }

        private async void Requests_OnTapped(object sender, EventArgs e)
        {
            var page = new Pages.PageRequests("Requests - ", UserType);
            page.CallbackEvent += PopupPageTaskClosed_CallbackEvent;
            await Navigation.PushAsync(page);
        }

        private async void Settings_OnTapped(object sender, System.EventArgs e)
        {
            var page = new Pages.PageSettings("Settings - ", UserType);
            page.CallbackEvent += PopupPageSettingsClosed_CallbackEvent;
            await Navigation.PushAsync(page);
        }

        private async void PopupPageSettingsClosed_CallbackEvent(object sender, object e)
        {
            FirebaseHelper.CurrentUser = await FirebaseHelper.GetUser(FirebaseHelper.CurrentUser.UserName);
            LoadSettings();
        }

        #endregion

        private async void Logout_OnTapped(object sender, System.EventArgs e)
        {
            FirebaseHelper.CurrentUser = null;
            await App.Current.MainPage.Navigation.PopAsync();
        }
     
    }
}

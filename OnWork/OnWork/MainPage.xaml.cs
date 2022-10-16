using OnWork.Models;
using Plugin.Geolocator;
using Rg.Plugins.Popup.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using OnWork.Infrastructure;
using OnWork.Infrastructure.Enums;
using OnWork.Pages;
using OnWork.Pages.Popup;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
namespace OnWork
{
    public partial class MainPage : ContentPage
    {
        private string PreviousPointId;
        private Color NavBackColor = Color.LightSlateGray;
        private Color BGDark;
        private Color BGLight;
        public EUserType UserType;
        private Distance DefaultDistance = Distance.FromMeters(400);
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

            if (Globals.Debug)
            { 
                //Tasks_OnTapped(this, null);
                //Task.Run(() => Tasks_OnTapped(this, null));
            }

            map.MapClicked += Map_MapClicked;

            Task.Run(() => LoadMapOnMyLocation());
            LoadPins();

            //var arr = new List<Position>(map.Pins.Select(x => x.Position));
            //arr.Add(GoogleMapsHelper.GetMyPosition());
           
            //DrawPoliline(arr.ToArray());
           
        }

        public void DrawPoliline(params Position[] positions)
        {
            var polyline = new Polyline
            {
                StrokeColor = Color.MediumPurple,
                StrokeWidth = 5,
                ZIndex = 0
            };

            foreach (var position in positions)
            {
                polyline.Positions.Add(position);
            }

            map.Polylines.Add(polyline);
        }
        
        [Obsolete]
        private void Map_MapClicked(object sender, MapClickedEventArgs e)
        {
            if (Globals.MapMode == MapMode.View) return;

            if (PreviousPointId != null)
                map.Pins.Remove(map.Pins.FirstOrDefault(x => x.Tag.ToString() == PreviousPointId));

            var place = PlacemarkHelper.GetPlacemarkAsync(e.Point);
            if (place == null) return;

            var pin = CreatePin(e.Point, $"Address: {place.Locality} {place.Thoroughfare} {place.SubThoroughfare}", "(Click here to select location)");
            PreviousPointId = pin.Tag.ToString();
            map.Pins.Add(pin);
            map.SelectedPin = pin;
            map.MoveToRegion(MapSpan.FromCenterAndRadius(pin.Position, DefaultDistance));
        }

        private void LoadSettings()
        {
            var settings = FirebaseHelper.CurrentUser?.Settings;
            Enum.TryParse(settings.MapType, out MapType type);
            map.MapType =  settings.MapType == null ? MapType.Satellite : type;
        }
        
        private void LoadMapOnMyLocation()
        {
            var position = GoogleMapsHelper.GetMyPosition();

            map.MoveToRegion(MapSpan.FromCenterAndRadius(position, DefaultDistance));
        }

        [Obsolete]
        private Pin CreatePin(Position location, string title, string desc, string id = null)
        {
            BitmapDescriptor icon = null;
            if (id == null)
            {
                id = Guid.NewGuid().ToString();
                icon = BitmapDescriptorFactory.FromBundle("pin");
            }
            else
            {
                icon = BitmapDescriptorFactory.FromBundle("base");
            }

            var pin = new Pin
            {
                Address = desc,
                IsVisible = true,
                Label = title,
                Tag = id,
                Icon = icon,
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
                     item.Desc + "(Click for additional info)",
                    item.id));
            }

            if(map.Pins.Count != 0)
                map.MoveToRegion(MapSpan.FromCenterAndRadius(map.Pins.Last().Position, DefaultDistance));
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
            var pin = (Pin) sender;
            var taskId = pin.Tag.ToString();
            var task = await FirebaseHelper.GetTaskItem(taskId);

            if (task != null)
            {
                var page = new Pages.Popup.PopupPageTask(task, UserType);
                page.CallbackEvent += PopupPageTaskClosed_CallbackEvent;
                await PopupNavigation.PushAsync(page);
            }
            else
            {
                Tasks_OnTapped(pin, null);
            }
        }

        [Obsolete]
        private void PopupPageTaskClosed_CallbackEvent(object sender, object e)
        {
            DefaultBackground();
            ReloadPins();
            UpdateMainPageControlsVisibility();
            LoadMapOnMyLocation();
        }

        private void UpdateMainPageControlsVisibility()
        {
            switch (Globals.MapMode)
            {
                case MapMode.View:
                    slHeader.IsVisible = true;
                    slFooter.IsVisible = true;

                    break;
                case MapMode.Select:
                    slHeader.IsVisible = false;
                    slFooter.IsVisible = false;

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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
                imgRoute.Source = ImageSource.FromResource("OnWork.Images.route.png");
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
            stckRoute.IsVisible = true;
            btnEmployee.BorderWidth = 2;
            btnEmployer.BorderWidth = 0;

            btnEmployee.BackgroundColor = BGDark;
            btnEmployer.BackgroundColor = BGLight;

            UserType = EUserType.Employee;

        }

        private void btnEmployer_Pressed(object sender, System.EventArgs e)
        {
            stckRoute.IsVisible = false;
            btnEmployer.BorderWidth = 2;
            btnEmployee.BorderWidth = 0;

            btnEmployer.BackgroundColor = BGDark;//Color.Red;
            btnEmployee.BackgroundColor = BGLight;

            UserType = EUserType.Employer;
        }

        [Obsolete]
        private async void Route_OnTapped(object sender, EventArgs e)
        {
            var page = new PopupPageCreateRoute();
            page.CallbackEvent += PopupClosed_CallbackEvent;
            await PopupNavigation.PushAsync(page);
        }

        private void PopupClosed_CallbackEvent(object sender, object e)
        {
        }

        #region footer

        [Obsolete]
        private async void Profile_OnTapped(object sender, System.EventArgs e)
        {
            var page = new Pages.PageProfile("Profile - ", UserType);
            page.CallbackEvent += PopupPageTaskClosed_CallbackEvent;
            await Navigation.PushAsync(page);
        }
        [Obsolete]
        private async void Tasks_OnTapped(object sender, System.EventArgs e)
        {
            PageTasks page;

            if (sender is Pin pin)
            {
                slFooter.IsVisible = true;
                page = new Pages.PageTasks("Tasks - ", UserType, pin);
            }
            else
            {
                page = new Pages.PageTasks("Tasks - ", UserType);
            }

            page.CallbackEvent += PopupPageTaskClosed_CallbackEvent;
            await Navigation.PushAsync(page);
        }

        [Obsolete]
        private async void Requests_OnTapped(object sender, EventArgs e)
        {
            var page = new Pages.PageRequests("Requests - ", UserType);
            page.CallbackEvent += PopupPageTaskClosed_CallbackEvent;
            await Navigation.PushAsync(page);
        }

        [Obsolete]
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

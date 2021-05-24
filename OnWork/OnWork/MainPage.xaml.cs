using OnWork.Models;
using System;
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
     
       
        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            LoadResources();
            btnEmployer_Pressed(this, null);
            #region footer
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
        }

        public void DefaultBackground()
        {
            stckHome.BackgroundColor = Color.White;
            stckAlarm.BackgroundColor = Color.White;
            stckCamera.BackgroundColor = Color.White;
            stckSettings.BackgroundColor = Color.White;
            stckLogout.BackgroundColor = Color.White;
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


        public EUserType UserType;

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
            await Navigation.PushAsync(new Pages.PageProfile("Profile - ", UserType));
        }

        private async void Tasks_OnTapped(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Pages.PageTasks("Tasks - ",UserType));
        }

        private async void Requests_OnTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Pages.PageRequests("Requests - ", UserType));
        }

        private async void Settings_OnTapped(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Pages.PageSettings("Settings - ", UserType));
        }

        #endregion

        private async void Logout_OnTapped(object sender, System.EventArgs e)
        {
            FirebaseHelper.CurrentUser = null;
            await App.Current.MainPage.Navigation.PopAsync();
        }
     
    }
}

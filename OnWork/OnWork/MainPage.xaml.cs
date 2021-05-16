using System.Windows.Input;
using Xamarin.Forms;

namespace OnWork
{
    public partial class MainPage : ContentPage
    {

        private Color NavBackColor = Color.LightSlateGray;
        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            btnEmployer_Pressed(this, null);
            #region footer
            imgHome.Source = ImageSource.FromResource("OnWork.Images.test.png");
            imgAlarm.Source = ImageSource.FromResource("XamarinForms_BottomNBar.alarm.png");
            imgCamera.Source = ImageSource.FromResource("XamarinForms_BottomNBar.camera.png");
            imgSettings.Source = ImageSource.FromResource("XamarinForms_BottomNBar.settings.png");
            imgLogout.Source = ImageSource.FromResource("XamarinForms_BottomNBar.logout.png");
            //Tap Gesture Recognizer  
            var homeTap = new TapGestureRecognizer();
            homeTap.Tapped += (sender, e) =>
            {
                DefaultBackground();
                stckHome.BackgroundColor = NavBackColor;
            };
            stckHome.GestureRecognizers.Add(homeTap);
            var alarmTap = new TapGestureRecognizer();
            alarmTap.Tapped += (sender, e) =>
            {
                DefaultBackground();
                stckAlarm.BackgroundColor = NavBackColor;
            };
            stckAlarm.GestureRecognizers.Add(alarmTap);
            var cameraTap = new TapGestureRecognizer();
            cameraTap.Tapped += (sender, e) =>
            {
                DefaultBackground();
                stckCamera.BackgroundColor = NavBackColor;
            };
            stckCamera.GestureRecognizers.Add(cameraTap);
            var settingsTap = new TapGestureRecognizer();
            settingsTap.Tapped += (sender, e) =>
            {
                DefaultBackground();
                stckSettings.BackgroundColor = NavBackColor;
            };
            stckSettings.GestureRecognizers.Add(settingsTap);
            var logoutTap = new TapGestureRecognizer();
            logoutTap.Tapped += (sender, e) =>
            {
                DefaultBackground();
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

        private void map_MyLocationButtonClicked(object sender, Xamarin.Forms.GoogleMaps.MyLocationButtonClickedEventArgs e)
        {

        }


        public enum EUserType { Employer, Employee }
        public EUserType UserType;

        private void btnEmployee_Pressed(object sender, System.EventArgs e)
        {
            btnEmployee.BorderWidth = 2;
            btnEmployer.BorderWidth = 0;

            btnEmployee.BackgroundColor = Color.Red;
            btnEmployer.BackgroundColor = Color.White;

            UserType = EUserType.Employee;

        }

        private void btnEmployer_Pressed(object sender, System.EventArgs e)
        {
            btnEmployer.BorderWidth = 2;
            btnEmployee.BorderWidth = 0;

            btnEmployer.BackgroundColor = Color.Red;
            btnEmployee.BackgroundColor = Color.White;

            UserType = EUserType.Employer;
        }

        #region OnTapped

        private async void Tasks_OnTapped(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Pages.PageTasks("Tasks - ",UserType));
        }

        private void Profile_OnTapped(object sender, System.EventArgs e)
        {

        }

        private void Home_OnTapped(object sender, System.EventArgs e)
        {

        }

        private void Settings_OnTapped(object sender, System.EventArgs e)
        {

        }

        #endregion

    }
}

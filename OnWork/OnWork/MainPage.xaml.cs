using Xamarin.Forms;

namespace OnWork
{
    public partial class MainPage : ContentPage
    {
        private Color NavBackColor = Color.LightSlateGray;
        public MainPage()
        {
            InitializeComponent();
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
        }
        public void DefaultBackground()
        {
            stckHome.BackgroundColor = Color.White;
            stckAlarm.BackgroundColor = Color.White;
            stckCamera.BackgroundColor = Color.White;
            stckSettings.BackgroundColor = Color.White;
            stckLogout.BackgroundColor = Color.White;
        }
    }
}

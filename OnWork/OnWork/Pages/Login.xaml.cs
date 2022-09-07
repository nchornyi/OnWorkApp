using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnWork.Infrastructure;
using OnWork.Models;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace OnWork.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            this.BindingContext = this;
            //this.BackgroundImageSource = "Images.location.png";

            if (Globals.Debug)
            {
                ELogin.Text = Globals.DebugUserName; EPassword.Text = Globals.DebugPassword;
                btnLogin_Clicked(this, null);
            }
        }

        private async void btnLogin_Clicked(object sender, EventArgs e)
        {
            if (IsBusy) return;

            if (Globals.Debug && e == null)
            {
                FirebaseHelper.CurrentUser = new User
                {
                    //id = "3cc5049b-7558-46ff-ab2f-c866d9826f59",
                    Info = new UserInfo()
                    {
                        MobileNumber = "+380983841818",
                        Description = "I am coder..."
                    },
                    Password = EPassword.Text,
                    Settings = new UserSettings
                    {
                        MapType = MapType.Hybrid.ToString()
                    },
                    UserName = ELogin.Text
                };

                var user1 = await FirebaseHelper.GetUser(ELogin.Text);
                FirebaseHelper.CurrentUser = user1;

                await Navigation.PushAsync(new MainPage());
                return;
            }

            this.IsBusy = true;
            string username = ELogin.Text ?? "", password = EPassword.Text ?? "";

            var user = await FirebaseHelper.GetUser(ELogin.Text);
            if(password == user?.Password && username == user?.UserName)
            {
                FirebaseHelper.CurrentUser = user;
                await Navigation.PushAsync(new MainPage());
            }
            else
                 await DisplayAlert("Error", "UserName or Password doesn't correct!", "OK");
            this.IsBusy = false;
        }

        private async void btnRegister_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Pages.Registration());
        }
    }
}
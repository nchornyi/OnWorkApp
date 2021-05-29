using Firebase.Database;
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
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            this.BindingContext = this;
            //this.BackgroundImageSource = "Images.location.png";

            ELogin.Text = "Nazar"; EPassword.Text = "123";
            //btnLogin_Clicked(this,null);
        }

        private async void btnLogin_Clicked(object sender, EventArgs e)
        {
            if (IsBusy) return;

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
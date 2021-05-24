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
    public partial class Registration : ContentPage
    {
        public Registration()
        {
            InitializeComponent();
            //  NavigationPage.SetHasNavigationBar(this, false);
            //      btnLogin_Clicked(this,null);
            this.BindingContext = this;

            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.Black;
            Title = nameof(Registration);
        }


        private async void btnRegister_Clicked(object sender, EventArgs e)
        {
            if (IsBusy) return;
            this.IsBusy = true;
            string login = ELogin.Text;
            if (!string.IsNullOrEmpty(login) && !string.IsNullOrEmpty(EPassword.Text))
            {
                var user = await FirebaseHelper.GetUser(login);

                if(user == null)
                {
                    await FirebaseHelper.AddUser(login, EPassword.Text);
                    await App.Current.MainPage.Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Warning", $"User with username {login} already exist", "OK");
                }
            }
            else
            {

            }
            this.IsBusy = false;
            //Navigation.PushAsync(new MainPage());
        }
    }
}
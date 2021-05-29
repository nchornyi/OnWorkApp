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
    public partial class PageProfile : ContentPage
    {
        private EUserType UserType;
        public event EventHandler<object> CallbackEvent;

        public PageProfile(string title, EUserType userType)
        {
            InitializeComponent();

            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.Black;
            UserType = userType;
            Title = title + userType.ToString();

            var info = Task.Run(async () => 
                await FirebaseHelper.GetUser(FirebaseHelper.CurrentUser.UserName))?.Result?.Info;

            eMobileNumber.Text = info.MobileNumber;
            eDescription.Text = info.Description;
            if (info.UserImage == null)
                userImage.Source = ImageSource.FromResource("OnWork.Images.user.png");
            //else
            //    userImage.Source = //ImageSource.FromFile(info.UserImage);
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            CallbackEvent?.Invoke(this, false);
        }

        private void btnAddImage(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            FirebaseHelper.UpdateUserInfo(FirebaseHelper.CurrentUser.UserName, new UserInfo()
            {
                UserImage = null,
                Description = eDescription.Text,
                MobileNumber = eMobileNumber.Text
            });
        }
    }
}
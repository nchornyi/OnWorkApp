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
        public PageProfile(string title, EUserType userType)
        {
            InitializeComponent();

            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.Black;
            UserType = userType;
            Title = title + userType.ToString();
        }
    }
}
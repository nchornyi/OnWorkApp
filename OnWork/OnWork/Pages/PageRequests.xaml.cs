using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using OnWork.Models;

namespace OnWork.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageRequests : ContentPage
    {
        private EUserType UserType;
        public PageRequests(string title, EUserType userType)
        {
            InitializeComponent();
            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.Black;
            UserType = userType;
            Title = title + userType.ToString();
        }
    }
}
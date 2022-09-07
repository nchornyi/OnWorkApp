using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using OnWork.Models;
using Rg.Plugins.Popup.Services;

namespace OnWork.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageRequests : ContentPage
    {
        private EUserType UserType;
        public event EventHandler<object> CallbackEvent;
        private List<TaskRequest> taskRequests = new List<TaskRequest>();
        public PageRequests(string title, EUserType userType)
        {
            InitializeComponent();
            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.Black;
            UserType = userType;
            Title = title + userType.ToString();

            LoadRequest();
        }

        private void LoadRequest()
        {
            var requests = FirebaseHelper.GetRequests(FirebaseHelper.CurrentUser.UserName, UserType);
            RequestsList.ItemsSource = taskRequests = requests;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            CallbackEvent?.Invoke(this, true);
        }
        [Obsolete]
        private async void RequestsList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null)
            {
                var request = (TaskRequest)e.Item;

                var page = new Popup.PopupPageRequest(request, UserType);
                page.CallbackEvent += PopupPageTaskClosed_CallbackEvent;
                await PopupNavigation.PushAsync(page);
            }
        }

        private void PopupPageTaskClosed_CallbackEvent(object sender, object e)
        {
            LoadRequest();
        }
    }
}
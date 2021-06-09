using OnWork.Models;
using Rg.Plugins.Popup.Services;
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
    public partial class PageTasks : ContentPage
    {
        public event EventHandler<object> CallbackEvent;
        public List<TaskItem> Tasks { get; set; } = new List<TaskItem>();
        private EUserType UserType;
        public PageTasks(string title, EUserType userType)
        {
            InitializeComponent();
            //NavigationPage.SetHasBackButton(this, false);
            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.Black;
            UserType = userType;
            Title = title + userType.ToString();
            if(UserType == EUserType.Employee)
            {
                btnAddItem.IsVisible = false;
            }

            LoadTasks();
            
            // Padding = new Thickness(0, 0, 0, 50);
            // BackgroundColor = Color.Transparent;
            ////await Navigation.PushAsync(new MainPage());
            //await Application.Current.MainPage.Navigation.PopAsync();
            ////await Application.Current.MainPage.Navigation.PopAsync();
        }

        public void LoadTasks()
        {
            try
            {
                TasksList.ItemsSource = Tasks = Task.Run(async () => await FirebaseHelper.GetTasks()).Result;
            }
            catch (Exception ex)
            {
            }
        }

        [Obsolete]
        private async void TasksList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null)
            {
                var page = new Popup.PopupPageTask((TaskItem)e.Item, UserType);
                page.CallbackEvent += PopupPageTaskClosed_CallbackEvent;
                await PopupNavigation.PushAsync(page);
            }
            //     selected.Text = e.Item.ToString();
            //  ((ListView)sender).SelectedItem = null;
        }

        [Obsolete]
        private async void btnAddItem_Clicked(object sender, EventArgs e)
        {
            var page = new Popup.PopupPageAddTask();
            page.CallbackEvent += PopupClosed_CallbackEvent;
            await PopupNavigation.PushAsync(page);
        }

        private void PopupClosed_CallbackEvent(object sender, object e)
        {
            if(e != null)
            {
                TasksList.BeginRefresh();

                var task = (TaskItem)e;
                Tasks.Add(task);
                TasksList.ItemsSource = Tasks;
                LoadTasks();

                TasksList.EndRefresh();
            }
        }

        private void PopupPageTaskClosed_CallbackEvent(object sender, object e)
        {
            var refresh = (bool)e;
            if(refresh)
                LoadTasks();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            CallbackEvent?.Invoke(this, true);
        }
    }
}
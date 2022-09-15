using OnWork.Models;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnWork.Infrastructure;
using OnWork.Infrastructure.Enums;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace OnWork.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageTasks : ContentPage
    {
        public event EventHandler<object> CallbackEvent;
        public List<TaskItem> Tasks { get; set; } = new List<TaskItem>();
        private EUserType UserType;

        [Obsolete]
        public PageTasks(string title, EUserType userType, Pin pin = null)
        {
            InitializeComponent();

            imgFilter.Source = ImageSource.FromResource(Globals.AppliedFilterItem == null 
                ? "OnWork.Images.filter.png" 
                : "OnWork.Images.unfilter.png");

            //NavigationPage.SetHasBackButton(this, false);
            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.Black;
            UserType = userType;
            Title = title + userType.ToString();
            if(UserType == EUserType.Employee)
            {
                btnAddItem.IsVisible = false;
            }

            LoadTasks();

            if (pin != null)
            {
                btnAddItem_Clicked(pin, null);
            }

            ////await Navigation.PushAsync(new MainPage());
            //await Application.Current.MainPage.Navigation.PopAsync();
            ////await Application.Current.MainPage.Navigation.PopAsync();
        }

        public void LoadTasks()
        {
            try
            {
                Tasks = Task.Run(async () => await FirebaseHelper.GetTasks()).Result;
                TasksList.ItemsSource = GetAllOrFilteredTasks();

            }
            catch (Exception ex)
            {
            }
        }

        [Obsolete]
        private async void TasksList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is TaskItem item)
            {
                var page = new Popup.PopupPageTask(item, UserType);
                page.CallbackEvent += PopupPageTaskClosed_CallbackEvent;
                await PopupNavigation.PushAsync(page);
            }
            //     selected.Text = e.Item.ToString();
            //  ((ListView)sender).SelectedItem = null;
        }

        [Obsolete]
        private async void btnAddItem_Clicked(object sender, EventArgs e)
        {
            var page = sender is Pin pin ? new Popup.PopupPageAddTask(pin) : new Popup.PopupPageAddTask();
            page.CallbackEvent += PopupClosed_CallbackEvent;
            await PopupNavigation.PushAsync(page);
        }
        [Obsolete]
        private async void Filter_OnTapped(object sender, EventArgs e)
        {
            if (Globals.AppliedFilterItem != null)
            {
                Globals.AppliedFilterItem = null;
                imgFilter.Source = ImageSource.FromResource("OnWork.Images.filter.png");
                TasksList.ItemsSource = Tasks;
            }
            else
            {
                var page = new Popup.PopupPageFilterTasks();
                page.CallbackEvent += PopupClosed_CallbackEvent;
                await PopupNavigation.PushAsync(page);
            }
        }

        private List<TaskItem> FilterTasks()
        {
            var filter = Globals.AppliedFilterItem;
            var filteredTasks = Tasks.Where(x => x.TaskType == filter.TaskType
                                                 && filter.Time.ContainsRange(x.Hours)
                                                 && filter.Price.ContainsValue(x.Price)
                                                 && filter.Distance > x.GetDistance()).ToList();

            return filteredTasks;
        }

        private List<TaskItem> GetAllOrFilteredTasks() => Globals.AppliedFilterItem == null ? Tasks : FilterTasks();

        private async void PopupClosed_CallbackEvent(object sender, object e)
        {
            if (e is FilterItem filter)
            {
                imgFilter.Source = ImageSource.FromResource("OnWork.Images.unfilter.png");
                Globals.AppliedFilterItem = filter;
                TasksList.BeginRefresh();
                
                TasksList.ItemsSource = FilterTasks();

                TasksList.EndRefresh();
            }
            else if (e is TaskItem task)
            {
                if (task.HasLocation())
                {
                    Globals.MapMode = MapMode.View;

                    TasksList.BeginRefresh();

                    Tasks.Add(task);
                    TasksList.ItemsSource = GetAllOrFilteredTasks();
                    LoadTasks();

                    TasksList.EndRefresh();
                }
                else
                {
                    Globals.MapMode = MapMode.Select;
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
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
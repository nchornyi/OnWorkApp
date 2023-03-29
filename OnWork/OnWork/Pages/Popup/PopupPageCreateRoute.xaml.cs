using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnWork.Infrastructure;
using OnWork.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms.GoogleMaps;

namespace OnWork.Pages.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupPageCreateRoute : Rg.Plugins.Popup.Pages.PopupPage, INotifyPropertyChanged
    {
        public PopupPageCreateRoute()
        {
            InitializeComponent();

            //pRouteType.ItemsSource = Enum.GetNames(typeof(ERouteSort));

            LoadRequest();

            var tasks = Task.Run(async () =>
            {
                TasksList.ItemsSource = CreateRouteItems();
                TasksList.RefreshItemsSource();
            });
            //TasksList.ItemsSource = CreateRoute();
            //TasksList.RefreshItemsSource();
        }

        public event EventHandler<object> CallbackEvent;
        public event PropertyChangedEventHandler PropertyChanged;
        public List<TaskItem> tasksList = new List<TaskItem>();

        private void LoadRequest()
        {
            var tasks = Task.Run(async () => await FirebaseHelper.GetTasks()).Result;
            var requests = FirebaseHelper.GetRequests(FirebaseHelper.CurrentUser.UserName, EUserType.Employee)
                .Where(x => x.Status == EStatus.Approved.ToString());

            var list = new List<TaskItem>();
            foreach (var task in tasks)
            {
                var intersect = task.Requests.Select(x=>x.id).Intersect(requests.Select(y=>y.id)).ToList();
                if (intersect.Any())
                {
                    list.Add(task);
                }
            }

            TasksList.ItemsSource = tasksList = FilterItems(list);
            lvIds.ItemsSource = Enumerable.Range(1, tasksList.Count).Select(x => x.ToString() + ")");
        }

        private List<TaskItem> FilterItems(List<TaskItem> items)
        {
            var filteredTaskItems = new List<TaskItem>();
            var ignore = new List<string>();

            try
            {
                foreach (var item in items)
                {
                    var delete = filteredTaskItems.Where(x => ignore.Contains(x.id)).ToList();
                    if (delete.Any())
                        filteredTaskItems = new List<TaskItem>(filteredTaskItems.Except(delete));

                    var inRange = items.Where(x => x.Hours.ContainsRange(item.Hours)).ToList();
                    if (!inRange.Any())
                    {
                        filteredTaskItems.Add(item);
                        continue;
                    }

                    var maxPriceTask = inRange.OrderByDescending(x => x.Price).FirstOrDefault();
                    ignore.AddRange(inRange.Where(x => x.id != maxPriceTask.id).Select(x => x.id));

                    if (item.id == maxPriceTask.id && !ignore.Contains(maxPriceTask.id))
                        filteredTaskItems.Add(item);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return filteredTaskItems;
        }

        private List<RouteInfo> GetRouteInfo()
        {
            var arr = new List<Position>(tasksList.Select(x=> new Position(x.TaskLocationItem.Location.Latitude,x.TaskLocationItem.Location.Longitude)));
            arr.Insert(0, GoogleMapsHelper.GetMyPosition());

            var list = new List<RouteInfo>();
            for (var x = 0; x < arr.Count - 1; x++)
            {
                var distance = GetDistance(arr[x], arr[x + 1]);
                list.Add(new RouteInfo(distance, GetTime(distance)));
            }

            return list;
        }

        private RouteInfo GetRouteInfo(TaskItem task)
        {
            var myPosition = GoogleMapsHelper.GetMyPosition();
            var taskPosition = new Position(task.TaskLocationItem.Location.Latitude, task.TaskLocationItem.Location.Longitude);
            var distance = GetDistance(myPosition, taskPosition);
            return new RouteInfo(distance, GetTime(distance));
        }

        public class RouteItem
        {
            public RouteItem(TaskItem task, RouteInfo info)
            {
                Task = task;
                Info = info;
            }
            public TaskItem Task { get; set; }
            public RouteInfo Info { get; set; }
        }

        public class RouteInfo
        {
            public RouteInfo(double distance, double time)
            {
                Distance = distance;
                Time = time;
            }

            public double Distance { get; set; }
            public double Time { get; set; }
        }

        private double GetDistance(Position pos1, Position pos2)
        {
            var loc1 = new Location(pos1.Latitude, pos1.Longitude);
            var loc2 = new Location(pos2.Latitude, pos2.Longitude);

            return loc1.CalculateDistance(loc2, DistanceUnits.Kilometers);
        }

        private double GetTime(double distance, double speed = 5.0) => 60 / (speed / distance);
        
        private async void btnApplyRoute_Clicked(object sender, EventArgs e)
        {
            this.IsBusy = false;
            await Navigation.PopPopupAsync();
        }

        private List<RouteItem> CreateRouteItems()
        {
            tasksList = new List<TaskItem>(tasksList.OrderBy(x => x.Hours.Minimum));
            Globals.RouteItems = tasksList;

            var list = new List<RouteItem>();
            foreach (var item in tasksList)
            {
                list.Add(new RouteItem(item, GetRouteInfo(item)));
            }

            return list;
        }

        private List<TaskItem> CreateRoute()
        {
            tasksList = new List<TaskItem>(tasksList.OrderBy(x => x.Hours.Minimum));

            //var type = Enum.Parse(typeof(ERouteSort), pRouteType.SelectedItem.ToString());
            //switch (type)
            //{
            //    case ERouteSort.Price:
            //        tasksList = new List<TaskItem>(tasksList.OrderByDescending(x => x.Price));

            //        break;
            //    case ERouteSort.Distance:
            //        tasksList = new List<TaskItem>(tasksList.OrderByDescending(x => x.GetDistance()));

            //        break;

            //    default:
            //        break;
            //}

            return Globals.RouteItems = tasksList;
        }

        private void PRouteType_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            TasksList.ItemsSource = tasksList;
            TasksList.RefreshItemsSource();
            btnApplyFilter.IsEnabled = true;
            btnApplyFilter.BackgroundColor = Color.LightGreen;
        }

        #region PopupEvents

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            CallbackEvent?.Invoke(this, tasksList);
            base.OnDisappearing();
        }

        // ### Methods for supporting animations in your popup page ###

        // Invoked before an animation appearing
        protected override void OnAppearingAnimationBegin()
        {
            base.OnAppearingAnimationBegin();
        }

        // Invoked after an animation appearing
        protected override void OnAppearingAnimationEnd()
        {
            base.OnAppearingAnimationEnd();
        }

        // Invoked before an animation disappearing
        protected override void OnDisappearingAnimationBegin()
        {
            base.OnDisappearingAnimationBegin();
        }

        // Invoked after an animation disappearing
        protected override void OnDisappearingAnimationEnd()
        {
            base.OnDisappearingAnimationEnd();
        }

        protected override Task OnAppearingAnimationBeginAsync()
        {
            return base.OnAppearingAnimationBeginAsync();
        }

        protected override Task OnAppearingAnimationEndAsync()
        {
            return base.OnAppearingAnimationEndAsync();
        }

        protected override Task OnDisappearingAnimationBeginAsync()
        {
            return base.OnDisappearingAnimationBeginAsync();
        }

        protected override Task OnDisappearingAnimationEndAsync()
        {
            return base.OnDisappearingAnimationEndAsync();
        }

        // ### Overrided methods which can prevent closing a popup page ###

        // Invoked when a hardware back button is pressed
        protected override bool OnBackButtonPressed()
        {
            // Return true if you don't want to close this popup page when a back button is pressed
            return base.OnBackButtonPressed();
        }

        // Invoked when background is clicked
        protected override bool OnBackgroundClicked()
        {
            // Return false if you don't want to close this popup page when a background of the popup page is clicked
            return base.OnBackgroundClicked();
        }
        #endregion

        #region Drag list events


        private void Ecv_OnSelectionChanging(object sender, ExtendedControls.ExtCollectionView.XForms.EventArgs.SelectedItemsChangingEventArgs args)
        {

        }

        private void Ecv_OnSelectionChanged(object sender, ExtendedControls.ExtCollectionView.XForms.EventArgs.SelectedItemsChangedEventArgs args)
        {

        }

        private void Ecv_OnChildrenReordered(object sender, EventArgs e)
        {

        }

        private void Ecv_OnItemDragStarted(object sender, ExtendedControls.ExtCollectionView.XForms.EventArgs.DragStartedEventArgs args)
        {

        }

        private void Ecv_OnItemIntersecting(object sender, ExtendedControls.ExtCollectionView.XForms.EventArgs.IntersectionEventArgs args)
        {

        }

        private void Ecv_OnItemDropped(object sender, ExtendedControls.ExtCollectionView.XForms.EventArgs.DropEventArgs args)
        {

        }

        private void Ecv_OnItemSourceChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs args)
        {

        }

        #endregion

       
    }
}
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

namespace OnWork.Pages.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupPageCreateRoute : Rg.Plugins.Popup.Pages.PopupPage, INotifyPropertyChanged
    {
        public PopupPageCreateRoute()
        {
            InitializeComponent();

            pRouteType.ItemsSource = Enum.GetNames(typeof(ERouteSort));

            LoadRequest();
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

            TasksList.ItemsSource = tasksList = list;
            lvIds.ItemsSource = Enumerable.Range(1, tasksList.Count).Select(x => x.ToString());
        }

        
        private void btnApplyRoute_Clicked(object sender, EventArgs e)
        {

        }

        private void PRouteType_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            tasksList.Reverse();
            //TasksList.ItemsSource = null;
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
            CallbackEvent?.Invoke(this, null);
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
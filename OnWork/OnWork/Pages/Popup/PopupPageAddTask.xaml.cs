using OnWork.Models;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnWork.Generics;
using OnWork.Infrastructure;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;
using Position = Plugin.Geolocator.Abstractions.Position;

namespace OnWork.Pages.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupPageAddTask : Rg.Plugins.Popup.Pages.PopupPage
    {
        public event EventHandler<object> CallbackEvent;
        private TaskItem NewTask = null;
        private Position position;

        public PopupPageAddTask(Pin pin = null)
        {
            InitializeComponent();

            imgSelectLocation.Source = ImageSource.FromResource("OnWork.Images.pin.png");
            pTaskType.ItemsSource = Globals.TaskTypesList;

            if (pin != null)
            {
                LoadNotFinishedTaskItem();

                position = new Position(pin.Position.Latitude, pin.Position.Longitude);
                var place = PlacemarkHelper.GetPlacemarkAsync(pin.Position);
                ELocation.Text = $"{place.Locality} {place.Thoroughfare} {place.SubThoroughfare}";
            }
        }

        private void LoadNotFinishedTaskItem()
        {
            ETitle.Text = Globals.NotFinishedTaskItem.Title;
            pTaskType.SelectedItem = Globals.NotFinishedTaskItem.TaskType;
            EDescription.Text = Globals.NotFinishedTaskItem.Desc;
            EPrice.Text = Globals.NotFinishedTaskItem.Price.ToString();
            rsTime.LowerValue = Globals.NotFinishedTaskItem.Hours.Minimum;
            rsTime.UpperValue = Globals.NotFinishedTaskItem.Hours.Maximum;
        }

        private async void Location_OnTapped(object sender, EventArgs e)
        {
            try
            {
                Globals.NotFinishedTaskItem = NewTask = new TaskItem(DateTime.Now)//true 
                {
                    OwnerNickName = FirebaseHelper.CurrentUser.UserName,
                    Title = ETitle.Text,
                    TaskType = pTaskType.SelectedItem?.ToString(),
                    Desc = EDescription.Text,
                    Hours = new Range<double>(rsTime.LowerValue, rsTime.UpperValue),
                    Price = string.IsNullOrEmpty(EPrice.Text) ? default : Convert.ToDouble(EPrice.Text)
                };
            }
            catch (Exception ex)
            {
            }

            await Navigation.PopPopupAsync();
        }

        private void BtnSelectLocation_OnClicked(object sender, EventArgs e)
        {
        }

        private void ItemSelected(object sender, ItemTappedEventArgs e)
        {
            ELocation.Text = ((TaskLocation)e.Item).Title;
            //lvLocations.IsVisible = false;
        }

        private async void btnAdd_Clicked(object sender, EventArgs e)
        {
            this.IsBusy = true;
            var item = new TaskItem(DateTime.Now) //true 
            {
                OwnerNickName = FirebaseHelper.CurrentUser.UserName,
                Title = ETitle.Text,
                Desc = EDescription.Text,
                TaskType = pTaskType.SelectedItem.ToString(),
                Hours = new Range<double>(rsTime.LowerValue, rsTime.UpperValue),
                TaskLocationItem = new TaskLocation(ELocation.Text, position),
                Price = Convert.ToDouble(EPrice.Text)
            };
            if (!item.IsValid())
            {
                await DisplayAlert("Alert", "All fields need to be filled!", "OK");
                return;
            }

            if (item.TaskLocationItem == null)
            {
                NewTask = null;
                await Navigation.PopPopupAsync();
                return;
            }

            Globals.NotFinishedTaskItem = null;

            //  MessagingCenter.Send<PopupPageAddTask, string>(this, "msg", "test");
            await FirebaseHelper.AddTaskItem(item);
            // Close the last PopupPage int the PopupStack
            NewTask = item;
            this.IsBusy = false;
            await Navigation.PopPopupAsync();
        }

        #region PppupEvents
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            CallbackEvent?.Invoke(this, NewTask);
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

        
    }
}

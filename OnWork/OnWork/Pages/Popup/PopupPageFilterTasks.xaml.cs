using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnWork.Generics;
using OnWork.Infrastructure;
using OnWork.Models;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OnWork.Pages.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupPageFilterTasks :  Rg.Plugins.Popup.Pages.PopupPage, INotifyPropertyChanged
    {

        private FilterItem filter = null;

        public event EventHandler<object> CallbackEvent;

        public PopupPageFilterTasks()
        {
            InitializeComponent();
            
            pTaskType.ItemsSource = Globals.TaskTypesList;
        }

        private void PTaskType_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            btnApplyFilter.IsEnabled = true;
            btnApplyFilter.BackgroundColor = Color.LightGreen;
        }
        
        private async void btnApplyFilter_Clicked(object sender, EventArgs e)
        {
            filter = new FilterItem(pTaskType.SelectedItem.ToString(),
                                    sKilometers.Value,
                                new Range<double>(rsPrice.LowerValue, rsPrice.UpperValue),
                                new Range<double>(rsTime.LowerValue, rsTime.UpperValue));

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
            CallbackEvent?.Invoke(this, filter);
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
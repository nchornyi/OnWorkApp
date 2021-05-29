﻿using OnWork.Models;
using Plugin.Geolocator.Abstractions;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OnWork.Pages.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupPageAddTask : Rg.Plugins.Popup.Pages.PopupPage
    {
        public event EventHandler<object> CallbackEvent;


        private List<TaskLocation> LocationList { get; set; }
        public PopupPageAddTask()
        {
            InitializeComponent();

            LocationList = new List<TaskLocation>()
            {
                new TaskLocation()
                {
                    Title = "Карма Кава",
                    Location = new Position(49.5523761,25.5925439)
                },
                new TaskLocation()
                {
                    Title = "Мамонт",
                    Location = new Position(49.5491325,25.5934022)
                },
                new TaskLocation()
                {
                    Title = "ЖД",
                    Location = new Position(49.5516383,25.5964921)
                },
                new TaskLocation()
                {
                    Title = "ФКІТ",
                    Location = new Position(49.5607243,25.5937135)
                },
                new TaskLocation()
                {
                    Title = "Гімназія",
                    Location = new Position(49.5526858,25.5990243)
                }
            };

            lvLocations.BindingContext = LocationList;
            lvLocations.ItemsSource = LocationList;
        }

        private void ItemSelected(object sender, ItemTappedEventArgs e)
        {
            ELocation.Text = ((TaskLocation)e.Item).Title;
            lvLocations.IsVisible = false;
        }
        private TaskItem NewTask = null;
        private async void btnAdd_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ETitle.Text) &&
                !string.IsNullOrWhiteSpace(EDescription.Text) &&
                !string.IsNullOrWhiteSpace(ELocation.Text) &&
                !string.IsNullOrWhiteSpace(EPrice.Text))
            {
                this.IsBusy = true;
                var item = new TaskItem(DateTime.Now)//true 
                {
                    OwnerNickName = FirebaseHelper.CurrentUser.UserName,
                    Desc = EDescription.Text,
                    Title = ETitle.Text,
                    TaskLocationItem = LocationList.FirstOrDefault(x=>x.Title== ELocation.Text),
                    Price = EPrice.Text
                };

                if(item.TaskLocationItem == null)
                {
                    NewTask = null;
                    await Navigation.PopPopupAsync();
                    return;
                }

                //  MessagingCenter.Send<PopupPageAddTask, string>(this, "msg", "test");
                await FirebaseHelper.AddTaskItem(item);
                // Close the last PopupPage int the PopupStack
                NewTask = item;
                this.IsBusy = false;
                await Navigation.PopPopupAsync();

            }
            else
            {
                //await Navigation.PopPopupAsync();//delete in release!!!
                await DisplayAlert("Alert", "All fields need to be filled!", "OK");
            }
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

using OnWork.Models;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Linq;
using System.Drawing;
using System.Threading.Tasks;
using Xamarin.Forms.Xaml;

namespace OnWork.Pages.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupPageTask : Rg.Plugins.Popup.Pages.PopupPage
    {
        public event EventHandler<object> CallbackEvent;

        public TaskItem item;
        private EUserType UserType;
        public PopupPageTask(TaskItem task, EUserType userType)
        {
            InitializeComponent();
            UserType = userType;
            item = task;
            OpenedTask.BindingContext = item;
            
            switch (userType)
            {
                case EUserType.Employer:
                    if (task.OwnerNickName == FirebaseHelper.CurrentUser.UserName)
                    {
                        btnSendRequest.Text = "Delete task";
                        btnSendRequest.BackgroundColor = Color.Red;
                    }
                    else
                    {
                        btnSendRequest.IsVisible = false;
                    }
                    break;
                case EUserType.Employee:

                    var userRequest = task.Requests.FirstOrDefault(x => x.UserNickName == FirebaseHelper.CurrentUser.UserName);
                    if (userRequest == null)
                    {
                        btnSendRequest.Text = "Send request";
                        btnSendRequest.BackgroundColor = Color.LightGreen;
                    }
                    else
                    {
                        btnSendRequest.Text = "Revert request";
                        btnSendRequest.BackgroundColor = Color.Gold;
                    }

                    break;
                default:
                    break;
            }
        }
        private async void btnSendRequest_Clicked(object sender, EventArgs e)
        {
            this.IsBusy = true;

            switch (UserType)
            {
                case EUserType.Employer:


                    var answer = true;//await DisplayAlert("Warning", "Do you really want to delete task?", "Yes", "No");
                    if (answer)
                    {
                        await FirebaseHelper.DeleteTaskItem(item);
                    }
                    else
                    {
                        return;
                    }
                    break;
                case EUserType.Employee:

                    var userRequest = item.Requests.FirstOrDefault(x => x.UserNickName == FirebaseHelper.CurrentUser.UserName);
                    if (userRequest == null)
                    {
                        var request = new TaskRequest() { UserNickName = FirebaseHelper.CurrentUser.UserName, Description = " " };
                        item.Requests.Add(request);
                        await FirebaseHelper.UpdateTaskItem(item);
                    }
                    else
                    {
                        item.Requests.Remove(userRequest);
                        await FirebaseHelper.UpdateTaskItem(item);
                    }

                    break;
                default:
                    break;
            }
            // Close the last PopupPage int the PopupStack

            this.IsBusy = false;
            await Navigation.PopPopupAsync();
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
    }
}

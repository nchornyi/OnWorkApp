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
        public List<TaskItem> Tasks { get; set; } = new List<TaskItem>();
        public class TaskItem
        {
            public string Title { get; set; }
            public string Desc { get; set; }
            public ImageSource LocationIcon { get; set; }
        }
        public PageTasks(string title, MainPage.EUserType userType)
        {
            InitializeComponent();
            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.Black;
            Title = title + userType.ToString();

            var locationIcon = ImageSource.FromResource("OnWork.Images.location.png");
            //  TasksList.Ima.Source = ImageSource.FromResource("OnWork.Images.test.png");

            if (Tasks.Count==0)
            Tasks = new List<TaskItem>
            {
                new TaskItem() { Title = "test1", Desc = "ww", LocationIcon = locationIcon},
                new TaskItem() { Title = "test2", Desc = "ww", LocationIcon = locationIcon},
                new TaskItem() { Title = "test3", Desc = "ww", LocationIcon = locationIcon}
            };

            //  Tasks = new string[] { "test1", "test2", "test3" };
            TasksList.ItemsSource = Tasks;
            // Padding = new Thickness(0, 0, 0, 50);
            // BackgroundColor = Color.Transparent;

            ////await Navigation.PushAsync(new MainPage());
            //await Application.Current.MainPage.Navigation.PopAsync();
            ////await Application.Current.MainPage.Navigation.PopAsync();
        }

        private void TasksList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null)
           //     selected.Text = e.Item.ToString();
            ((ListView)sender).SelectedItem = null;
        }
    }
}
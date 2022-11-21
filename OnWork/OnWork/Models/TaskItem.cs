using System;
using System.Collections.Generic;
using OnWork.Generics;
using Xamarin.Forms;
using OnWork.Infrastructure;
using Xamarin.Essentials;
using Xamarin.Forms.GoogleMaps;

namespace OnWork.Models
{
    public class TaskItem
    {
        public string id { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public string TaskType { get; set; }
        public Range<double> Hours { get; set; }
        public TaskLocation TaskLocationItem { get; set; }
        public double Price { get; set; }
        public string DateCreated { get; set; }
        public string OwnerNickName { get; set; }
        public List<TaskRequest> Requests { get; set; } = new List<TaskRequest>();
        public ImageSource LocationIcon { get; set; }

        public TaskItem() : this(true)
        {
             
        }
        public TaskItem(DateTime created) : this(false)
        {
            id = Guid.NewGuid().ToString();
            DateCreated = created.ToString();
        }
        public TaskItem(bool IsLocal)
        {
            if (IsLocal)
            {
                LocationIcon = ImageSource.FromResource("OnWork.Images.location.png");
            }
        }

        public bool HasLocation() => TaskLocationItem != null;

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Title) && !string.IsNullOrEmpty(Desc) && !double.IsNaN(Price) &&
                    !string.IsNullOrEmpty(DateCreated) && !string.IsNullOrEmpty(OwnerNickName);
        }

        public double GetDistance()
        {
            var position = GoogleMapsHelper.GetMyPosition();
            var myLocation = new Location(position.Latitude, position.Longitude);
            var taskLocation = new Location(TaskLocationItem.Location.Latitude, TaskLocationItem.Location.Longitude);
            double distance = taskLocation.CalculateDistance(myLocation, DistanceUnits.Kilometers);

            return distance;
        }

        public Position GetPosition() => HasLocation() 
            ? new Position(TaskLocationItem.Location.Latitude, TaskLocationItem.Location.Longitude)
            : new Position();

    }
}

using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace OnWork.Models
{
    public class TaskItem
    {
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
        
        public string id { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public TaskLocation TaskLocationItem { get; set; }
        public string Price { get; set; }
        public string DateCreated { get; set; }
        public string OwnerNickName { get; set; }
        public List<TaskRequest> Requests { get; set; } = new List<TaskRequest>();
        public ImageSource LocationIcon { get; set; }
    }
}

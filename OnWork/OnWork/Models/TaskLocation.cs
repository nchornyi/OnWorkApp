using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnWork.Models
{
    public class TaskLocation
    {
        public TaskLocation(string title, Position location)
        {
            Title = title;
            Location = location;
        }

        public string Title { get; set; }
        public Position Location { get; set; }
    }
}

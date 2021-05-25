using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnWork.Models
{
    public class TaskLocation
    {
        public string Title { get; set; }
        public Position Location { get; set; }
    }
}

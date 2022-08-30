using System;
using System.Collections.Generic;
using System.Text;
using OnWork.Generics;

namespace OnWork.Models
{
    public class FilterItem
    {
        public FilterItem()
        {
            
        }

        public FilterItem(string taskType, double distance, Range<double> price, Range<double> time)
        {
            TaskType = taskType;
            Distance = distance;
            Price = price;
            Time = time;
        }

        public string TaskType { get; set; }
        public double Distance { get; set; }
        public Range<double> Price { get; set; }
        public Range<double> Time { get; set; }
    }
}

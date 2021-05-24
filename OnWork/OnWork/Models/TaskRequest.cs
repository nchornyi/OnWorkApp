using System;
using System.Collections.Generic;
using System.Text;

namespace OnWork.Models
{
    public class TaskRequest
    {
        public TaskRequest()
        {
            id = Guid.NewGuid().ToString();
            RequestDate = DateTime.Now.ToString();
        }

        public string id { get; set; }
        public string UserId { get; set; }
        public string RequestDate { get; set; }
        public string Description { get; set; }
    }
}

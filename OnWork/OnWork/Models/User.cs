using System;
using System.Collections.Generic;
using System.Text;

namespace OnWork.Models
{
    public class User
    {
        public User()
        {
            id = Guid.NewGuid().ToString();
        }
        public string id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}

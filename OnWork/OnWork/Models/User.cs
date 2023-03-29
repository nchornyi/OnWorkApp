using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

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
        public UserInfo Info { get; set; } = new UserInfo();
        public UserSettings Settings { get; set; } = new UserSettings();
    }
}

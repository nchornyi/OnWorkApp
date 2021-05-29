using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace OnWork.Models
{
    public class UserInfo
    {
        public UserInfo()
        {
        }
        public Image UserImage { get; set; }
        public string NickName { get; set; }
        public string MobileNumber { get; set; }
        public string Description { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace OnWork.Models
{
    public enum EUserType { Employer, Employee }
    public enum ETables { Tasks, Requests, Users}
    public enum EStatus { Approved, Declined, InReview = -1 }
    public enum ERouteSort { Price, Distance }// Tasks,
}

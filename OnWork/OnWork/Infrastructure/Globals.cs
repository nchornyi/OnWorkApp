using System;
using System.Collections.Generic;
using System.Text;
using OnWork.Infrastructure.Enums;
using OnWork.Models;

namespace OnWork.Infrastructure
{
    public static class Globals
    {
        public static bool Debug { get; set; } = true;
        public static string DebugUserName { get; set; } = "Vadim";
        //public static string DebugUserName { get; set; } = "Nazar";
        public static string DebugPassword { get; set; } = "1";
        public static MapMode MapMode { get; set; } = MapMode.View;
        public static FilterItem AppliedFilterItem = null;
        public static TaskItem NotFinishedTaskItem = null;
        public static List<string> TaskTypesList = new List<string>
        {
            "Няня",
            "Вигул тварин",
            "Прибирання",
            "Ремонтні роботи"
        };
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiApp1.Models.Enums;

namespace MauiApp1.Models
{
    public class Reminders
    {
        public string ReminderID { get; set; }
        public ReminderTypeEnum ReminderType { get; set; }
        public string Description { get; set; }
        public bool Completed { get; set;}
        public PriorityEnum Priority { get; set; }


    }
}

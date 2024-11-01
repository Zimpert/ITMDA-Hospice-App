namespace MauiApp1.Models
{
    public class NotificationItem
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; } // File path or resource name of the icon
        public DateTime Date { get; set; } // Date of the notification
    }
}

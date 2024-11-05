using MauiApp1.PartialViews;
using MauiApp1.Models;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MauiApp1.Views
{
    public partial class NotificationPage : ContentPage
    {
        public NotificationPage()
        {
            InitializeComponent();
            LoadNotifications();
        }


        private async void OnBackButtonTapped(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("///HomePage"); // Navigate to the Homepage
        }

        private async void OnSettingsIconTapped(object sender, EventArgs e)
        {

            await Shell.Current.GoToAsync("///SettingsPage"); // Navigate to the SettingsPage
        }


        private void LoadNotifications()
        {
            // Clear existing notifications
            NotificationListContainer.Children.Clear();

            // Get notifications (this is a mock data example; replace with actual data)
            var notifications = GetMockNotifications();

            // Group notifications by date
            var today = DateTime.Today;
            var groupedNotifications = notifications
                .GroupBy(n => n.Date.Date == today ? "Today" :
                              n.Date.Date == today.AddDays(-1) ? "Yesterday" :
                              n.Date.ToString("MMMM d, yyyy"));

            foreach (var group in groupedNotifications)
            {
                // Add a header label for each group (e.g., Today, Yesterday, October 28, 2024)
                NotificationListContainer.Children.Add(new Label
                {
                    Text = group.Key,
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 16,
                    TextColor = Colors.Black, // Match container background color
                    Margin = new Thickness(0, 10, 0, 5)
                });

                // Add each notification item in the group
                foreach (var notification in group)
                {
                    var notificationItemView = new NotificationItemView
                    {
                        Title = notification.Title,
                        Description = notification.Description,
                        Icon = notification.Icon
                    };
                    NotificationListContainer.Children.Add(notificationItemView);
                }
            }
        }

        private List<NotificationItem> GetMockNotifications()
        {
            // Placeholder data; in a real app, fetch notifications from a database or API
            return new List<NotificationItem>
            {
                new NotificationItem { Title = "Ms. Jones 9:30 AM", Description = "Doctor's Appointment for today", Icon = "placeholder.png", Date = DateTime.Today },
                new NotificationItem { Title = "Medication Reminder", Description = "Ibuprofen - Take with food", Icon = "placeholder.png", Date = DateTime.Today },
                new NotificationItem { Title = "Ms. Smith 11:00 AM", Description = "Follow-up Appointment", Icon = "placeholder.png", Date = DateTime.Today.AddDays(-1) },
                new NotificationItem { Title = "Ibuprofen Reminder", Description = "For yesterday", Icon = "placeholder.png", Date = DateTime.Today.AddDays(-1) },
                new NotificationItem { Title = "Prescription Update", Description = "New dosage information", Icon = "placeholder.png", Date = new DateTime(2024, 10, 28) }
            };
        }
    }
}

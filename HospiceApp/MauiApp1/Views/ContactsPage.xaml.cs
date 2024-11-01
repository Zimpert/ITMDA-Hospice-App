using MauiApp1.Models;
using MauiApp1.PartialViews;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using MauiApp1;

namespace MauiApp1
{
    public partial class ContactsPage : ContentPage
    {
        public ContactsPage()
        {
            InitializeComponent();
            LoadContacts();
        }

        private void LoadContacts()
        {
            // Clear existing contacts
            ContactsListContainer.Children.Clear();

            // Mock data for demonstration; replace with actual data source later
            var contacts = new List<ContactItem>
            {
                new ContactItem { Name = "Alice", Surname = "Nolan", Gender = "Female", Age = 29 },
                new ContactItem { Name = "Dylan", Surname = "Duncan", Gender = "Male", Age = 82 },
                new ContactItem { Name = "Anna", Surname = "Smith", Gender = "Female", Age = 75 },
                new ContactItem { Name = "Hannah", Surname = "Tucker", Gender = "Male", Age = 32 }
            };

            // Add each contact to the container
            foreach (var contact in contacts)
            {
                var contactItemView = new ContactItemView
                {
                    Name = $"{contact.Name} {contact.Surname}",
                    Details = $"{contact.Gender}, {contact.Age} years old"
                };
                ContactsListContainer.Children.Add(contactItemView);
            }
        }
    }
}

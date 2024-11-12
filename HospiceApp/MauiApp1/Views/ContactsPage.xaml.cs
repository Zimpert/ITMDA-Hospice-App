using MauiApp1.PartialViews;
using MauiApp1.ViewModels;

namespace MauiApp1.Views
{
    public partial class ContactsPage : ContentPage
    {

        private readonly ContactPageViewModel _viewModel;

        public ContactsPage( ContactPageViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
            _viewModel = viewModel;

            _viewModel.ContactItemsCreated += OnContactsCreated;
            
        }

        private void OnContactsCreated()
        {
            LoadContacts();
        }

        private async void OnBackButtonTapped(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("///HomePage"); // Navigate to the Homepage
        }

        private async void OnSettingsIconTapped(object sender, EventArgs e)
        {
            // Navigate to the Settings page 
            await Shell.Current.GoToAsync("///SettingsPage"); // Navigate to the SettingsPage
        }

        public void LoadContacts()
        {
            // Clear existing contacts
            ContactsListContainer.Children.Clear();


            // Add each contact to the container
            if (_viewModel.ContactItemList != null)
            {
                foreach (var contact in _viewModel.ContactItemList)
                {
                    var contactItemView = new ContactItemView
                    {
                        BindingContext = contact
                    };
                    ContactsListContainer.Children.Add(contactItemView);
                }
            }
        }
    }
}

using MauiApp1.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;
using MauiApp1.Interfaces;

namespace MauiApp1.ViewModels
{
    
    public partial class ProfileViewModel : ObservableObject
    {
        private readonly IRequestManager _requestManager;

        [ObservableProperty]
        private User user;

        public ProfileViewModel(IRequestManager requestManager)
        {
            _requestManager = requestManager;
            User = new User();
        }



        /// <summary>
        /// This method is called OnAppearing because it is intended to be invoked when the view associated with this ViewModel appears on the screen.
        /// In a .NET MAUI application, this typically corresponds to the lifecycle event when a page becomes visible to the user.
        /// It is a common practice to load or refresh data in such methods to ensure the UI is up-to-date with the latest information.
        /// </summary>
        public async void OnAppearing()
        {
            var userID = await SecureStorage.GetAsync("userID");
            var token = await SecureStorage.GetAsync("authToken");

            if (string.IsNullOrEmpty(userID) || string.IsNullOrEmpty(token))
            {
                Console.WriteLine("User ID or token is missing.");
                return;
            }
            else
            {
                var userData = await _requestManager.GetUserDataAsync(userID, token);
                if (userData != null)
                {
                    User = userData;
                }
            }
        }
    }
}

using MauiApp1.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;
using MauiApp1.Interfaces;
using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;

namespace MauiApp1.ViewModels
{
    
    public partial class ProfileViewModel : ObservableObject
    {
        private readonly IRequestManager _requestManager;

        [ObservableProperty]
        private User? user; // Marked as nullable

        public ProfileViewModel(IRequestManager requestManager)
        {
            _requestManager = requestManager;
            InitializeUserAsync();
        }

        /// <summary>
        /// Initializes the user data from secure storage.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task InitializeUserAsync()
        {
            try
            {
                User currentUser = new User
                {
                    Name = await SecureStorage.GetAsync("Name") ?? string.Empty,
                    Surname = await SecureStorage.GetAsync("Surname") ?? string.Empty,
                    Email = await SecureStorage.GetAsync("Email") ?? string.Empty,
                    Address = await SecureStorage.GetAsync("Address") ?? string.Empty,
                    PhoneNo = await SecureStorage.GetAsync("PhoneNo") ?? string.Empty
                };

                User = currentUser;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error initializing user: {ex.Message}");
            }
        }


        public async Task OnAppearingAsync()
        {
            try
            {
                var userID = await SecureStorage.GetAsync("UserID");
                var token = await SecureStorage.GetAsync("Token");

                if (string.IsNullOrEmpty(userID) || string.IsNullOrEmpty(token))
                {
                    Debug.WriteLine("User ID or token is missing.");
                    return;
                }

                User? userData = await _requestManager.GetUserDataAsync(userID, token);
                if (userData != null)
                {
                    User = userData;
                }
                else
                {
                    Debug.WriteLine("Failed to retrieve user data.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in OnAppearingAsync: {ex.Message}");
            }
        }
    }
}

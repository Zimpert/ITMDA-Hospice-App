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

        // Backing field for the User property to store the user data
        [ObservableProperty]
        private User? user; // Marked as nullable

        public ProfileViewModel(IRequestManager requestManager)
        {
            _requestManager = requestManager;
            user = new User(); // Initialize the user field
        }


        public async Task OnAppearingAsync()
        {
            var userID = await SecureStorage.GetAsync("UserID");
            var token = await SecureStorage.GetAsync("Token");

            if (string.IsNullOrEmpty(userID) || string.IsNullOrEmpty(token))
            {
                Debug.WriteLine("User ID or token is missing.");
                return;
            }
            else
            {
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
        }
    }
}

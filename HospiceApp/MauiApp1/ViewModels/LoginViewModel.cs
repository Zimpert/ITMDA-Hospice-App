using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Interfaces;
using MauiApp1.Models;
using MauiApp1.Models.PatientModels;
using System.Diagnostics;


namespace MauiApp1.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly IRequestManager _requestManager;

        public LoginViewModel(IRequestManager requestManager)
        {
            _requestManager = requestManager;
            // Test to see if working
        }

        [ObservableProperty]
        string loginText;

        [ObservableProperty]
        string passwordText;

        [RelayCommand]
        public async Task Login()
        {
            Debug.WriteLine("Login method called."); // Add this line to verify the method is called
            var loginResult = await _requestManager.Login("/login", loginText, passwordText);
            if (loginResult != null)
            {
                Debug.WriteLine("Login successful."); // Add this line to verify the login was successful
                                                      // Navigate to the appropriate page based on the user's role
                await Shell.Current.GoToAsync("//HomePage");
            }
            else
            {
                Debug.WriteLine("Login failed."); // Add this line to verify the login failed
            }
        }
    }
}

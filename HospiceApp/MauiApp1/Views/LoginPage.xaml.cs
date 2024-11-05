using MauiApp1.Classes;
using MauiApp1.Interfaces;
using MauiApp1.Services;
using MauiApp1.ViewModels;
using System.Diagnostics;

namespace MauiApp1.Views
{
    public partial class LoginPage : ContentPage
    {
        private readonly IRequestManager _requestManager;
        public LoginPage(LoginViewModel viewModel, IRequestManager requestManager)
        {
            InitializeComponent();
            BindingContext = viewModel;
            _requestManager = requestManager;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            Debug.WriteLine("AppShell appearing.");
            
            await CheckLoginStatus();
        }
        private async Task CheckLoginStatus()
        {
            await Task.Delay(2000);
            Debug.WriteLine("Checking login status...");
            try
            {
                var authToken = await SecureStorage.GetAsync("Token");
                Debug.WriteLine($"Auth token retrieved: {authToken}");

                if (!await _requestManager.ValidateToken())
                {
                    Debug.WriteLine("Token validation failed. Navigating to LoginPage.");
                    await Shell.Current.GoToAsync("///LoginPage");
                }
                else
                {
                    Debug.WriteLine("Token validated successfully. Navigating to HomePage.");
                    await Shell.Current.GoToAsync("///HomePage");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in CheckLoginStatus: {ex.Message}");
            }
        }
    }
}


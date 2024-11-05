using Android.App;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Interfaces;
using System.Diagnostics;


namespace MauiApp1.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly IRequestManager _requestManager;

        public LoginViewModel(IRequestManager requestManager)
        {
            _requestManager = requestManager;
           
        }

        [ObservableProperty]
        string loginText;

        [ObservableProperty]
        string passwordText;

        [RelayCommand]
        public async Task Login()
        {
            Console.WriteLine("Login method called.");
            var loginResult = await _requestManager.LoginAsync(LoginText, PasswordText);
            await SecureStorage.SetAsync("TEST_STUFF", "TEST VALUE OUTPUT");
            var contentString = await SecureStorage.GetAsync("TEST_STUFF");
            await Shell.Current.DisplayAlert("Alert Title", contentString, "OK");
            if (loginResult != null)
            {
                Console.WriteLine("Login successful."); 

                if (Shell.Current != null)
                {
                    await SecureStorage.SetAsync("Token", loginResult.Token);
                    await SecureStorage.SetAsync("UserID", loginResult.UserID);
                    contentString = await SecureStorage.GetAsync("Token");
                    await Shell.Current.DisplayAlert("Alert Title", contentString, "OK");
                    await Shell.Current.GoToAsync("///HomePage"); // Navigate to the HomePage
                    Debug.WriteLine(await SecureStorage.GetAsync("TEST_STUFF"));
                    Debug.WriteLine(await SecureStorage.GetAsync("UserID"));
                    Debug.WriteLine(await SecureStorage.GetAsync("Token"));
                }
                else
                {
                    Console.WriteLine("Shell.Current is null.");
                }
            }
            else
            {
                Console.WriteLine("Login failed.");
                
            }
        }
    }
}

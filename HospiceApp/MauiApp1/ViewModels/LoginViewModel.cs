
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
            try
            {
                var loginResult = await _requestManager.LoginAsync(LoginText, PasswordText);

                if (loginResult != null)
                {
                    Console.WriteLine("Login successful.");

                    if (Shell.Current != null)
                    {
                        await SecureStorage.SetAsync("Token", loginResult.Token);
                        await SecureStorage.SetAsync("UserID", loginResult.UserID);
                        await SecureStorage.SetAsync("Role", loginResult.Role);
                        await SecureStorage.SetAsync("Email", loginResult.Email);
                        await SecureStorage.SetAsync("Name", loginResult.Name);
                        await SecureStorage.SetAsync("Surname", loginResult.Surname);
                        await SecureStorage.SetAsync("PhoneNo", loginResult.PhoneNo);
                        await SecureStorage.SetAsync("Address", loginResult.Address);

                        await Shell.Current.GoToAsync("///HomePage"); // Navigate to the HomePage
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
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred during login: {ex.Message}");
            }
        }
    }
}

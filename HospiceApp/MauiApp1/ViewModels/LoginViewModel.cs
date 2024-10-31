using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Interfaces;
using System.Diagnostics;

public partial class LoginViewModel : ObservableObject
{
    private readonly IRequestManager _requestManager;

    [ObservableProperty]
    private string loginText;

    [ObservableProperty]
    private string passwordText;

    public LoginViewModel(IRequestManager requestManager)
    {
        _requestManager = requestManager;
    }

    [RelayCommand]
    public async Task Login()
    {
        Debug.WriteLine("Login method called.");
        var loginResult = await _requestManager.LoginAsync(LoginText, PasswordText);

        if (loginResult != null)
        {
            if (Shell.Current != null)
            {
                await SecureStorage.SetAsync("Token", loginResult.Token);
                Debug.WriteLine(await SecureStorage.GetAsync("Token"));
                await SecureStorage.SetAsync("UserID", loginResult.UserID);
                await Shell.Current.GoToAsync("///HomePage");
            }
            else
            {
                Debug.WriteLine("Shell.Current is null.");
            }
        }
        else
        {
            Debug.WriteLine("Login failed.");
        }
    }
}

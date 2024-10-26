using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Interfaces;


namespace MauiApp1.ViewModels
{
    public partial class LoginViewModel :ObservableObject
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
            var loginResult = await _requestManager.Login("/login", loginText, passwordText);
            if (true)
            {
                Console.WriteLine("Login successful.");
                App.Current.MainPage = new HomePage();
                
            }
            else
            {
                Console.WriteLine("Login failed.");
            }
        }
    }
}

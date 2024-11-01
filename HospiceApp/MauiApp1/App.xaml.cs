using MauiApp1.Interfaces;
using System.Diagnostics;

namespace MauiApp1
{
    public partial class App : Application
    {
        private readonly IRequestManager _requestManager;

        public App(IServiceProvider serviceProvider, IRequestManager requestManager)
        {
            InitializeComponent();
            _requestManager = requestManager;
            
            MainPage = new AppShell();
            Debug.WriteLine("App initialized.");
        }

        protected override async void OnStart()
        {
            base.OnStart();
            Debug.WriteLine("App started.");
            try
            {
                await CheckLoginStatus();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in OnStart: {ex.Message}");
            }
        }

        private async Task CheckLoginStatus()
        {
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
    //public partial class App : Application
    //{
    //    private readonly IRequestManager _requestManager;

    //    /// <summary>
    //    /// Initializes a new instance of the <see cref="App"/> class.
    //    /// </summary>
    //    /// <param name="serviceProvider">The service provider.</param>
    //    /// <param name="requestManager">The request manager.</param>
    //    public App(IServiceProvider serviceProvider, IRequestManager requestManager)
    //    {
    //        InitializeComponent();
    //        _requestManager = requestManager;
    //        MainPage = new AppShell();
    //        _ = CheckLoginStatusAsync(serviceProvider);
    //    }

    //    /// <summary>
    //    /// Checks the login status and navigates to the appropriate page.
    //    /// </summary>
    //    /// <param name="serviceProvider">The service provider.</param>
    //    private async Task CheckLoginStatusAsync(IServiceProvider serviceProvider)
    //    {
    //        using (var client = new HttpClient())
    //        {
    //            Debug.WriteLine("Making GET request to http://example.com/");
    //            var response = await client.GetStringAsync("http://ddnd.crabdance.com/prelogin/");
    //            Debug.WriteLine($"Response from http://example.com/: {response}");
    //        }

    //        if (!await _requestManager.ValidateToken())
    //        {

    //            await Shell.Current.GoToAsync("///LoginPage");
    //        }
    //        else
    //        {
    //            await Shell.Current.GoToAsync("///HomePage");
    //        }
    //    }
    //}
}
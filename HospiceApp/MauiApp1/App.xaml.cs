using MauiApp1.Interfaces;

namespace MauiApp1
{
    public partial class App : Application
    {
        private readonly IRequestManager _requestManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="requestManager">The request manager.</param>
        public App(IServiceProvider serviceProvider, IRequestManager requestManager)
        {
            InitializeComponent();
            _requestManager = requestManager;
            MainPage = new AppShell();
            CheckLoginStatus(serviceProvider);
        }

        /// <summary>
        /// Checks the login status and navigates to the appropriate page.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        private async void CheckLoginStatus(IServiceProvider serviceProvider)
        {
            var authToken = await SecureStorage.GetAsync("Token");
            if (!await _requestManager.ValidateToken())
            {
                await Shell.Current.GoToAsync("///ScheduleViewer");
            }
            else
            {
                await Shell.Current.GoToAsync("///HomePage");
            }
        }
    }
}

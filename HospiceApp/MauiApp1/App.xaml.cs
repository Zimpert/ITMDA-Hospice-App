using MauiApp1.Interfaces;

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
        }

        protected override async void OnStart()
        {
            base.OnStart();
            await CheckLoginStatus();
        }

        private async Task CheckLoginStatus()
        {
            var authToken = await SecureStorage.GetAsync("Token");
            if (!await _requestManager.ValidateToken())
            {
                await Shell.Current.GoToAsync("///LoginPage");
            }
            else
            {
                await Shell.Current.GoToAsync("///HomePage");
            }
        }
    }

}

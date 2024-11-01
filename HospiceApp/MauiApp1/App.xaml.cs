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
            var authToken = "dac36fac-635c-40ef-95c9-0abce7ac7ac4";
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

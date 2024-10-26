namespace MauiApp1
{
    public partial class App : Application
    {
        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            var isLoggedIn = Preferences.Get("IsLoggedIn", false);
            if (isLoggedIn)
            {
                MainPage = new AppShell();
            }
            else
            {
                MainPage = serviceProvider.GetRequiredService<LoginPage>();
            }
        }
    }
}

namespace MauiApp1
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Check if the user is logged in
            bool isLoggedIn = Preferences.Get("IsLoggedIn", false);

            // Navigate to the appropriate page based on login status
            if (isLoggedIn)
            {
                GoToAsync("//HomePage");
            }
            else
            {
                GoToAsync("//LoginPage");
            }
        }
    }
}

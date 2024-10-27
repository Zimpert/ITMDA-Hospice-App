namespace MauiApp1
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            bool isLoggedIn = Preferences.Get("IsLoggedIn", false);

            if (isLoggedIn)
            {
                GoToAsync("//ProfilePage"); // change to homepage later
            }
            else
            {
                GoToAsync("//LoginPage");
            }
        }
    }
}

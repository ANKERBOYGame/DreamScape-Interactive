using DreamScape_Interactive.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace DreamScape_Interactive
{
    /// <summary>
    /// Home page shown after a successful login.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (SessionManager.CurrentUser != null)
                WelcomeText.Text = $"Welkom, {SessionManager.CurrentUser.Username}!";
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            SessionManager.EndSession();

            // Navigate back to login and clear the back-stack so the user
            // cannot navigate back to the home page without logging in again.
            Frame.Navigate(typeof(LoginPage));
            Frame.BackStack.Clear();
        }
    }
}

using DreamScape_Interactive.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

namespace DreamScape_Interactive
{
    /// <summary>
    /// Login page: accepts username + password and starts a session on success.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            AttemptLogin();
        }

        private void InputBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
                AttemptLogin();
        }

        private void AttemptLogin()
        {
            var username = UsernameBox.Text.Trim();
            var password = PasswordBox.Password;

            // Basic empty-field validation
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ShowError("Vul uw gebruikersnaam en wachtwoord in.");
                return;
            }

            var user = UserService.Instance.Authenticate(username, password);
            if (user == null)
            {
                ShowError("Onjuiste gebruikersnaam of wachtwoord. Probeer het opnieuw.");
                PasswordBox.Password = string.Empty;
                PasswordBox.Focus(FocusState.Programmatic);
                return;
            }

            // Credentials are correct – start the session and navigate to the home page.
            SessionManager.StartSession(user);
            Frame.Navigate(typeof(HomePage));
        }

        private void ShowError(string message)
        {
            ErrorMessageText.Text = message;
            ErrorMessageText.Visibility = Visibility.Visible;
        }
    }
}

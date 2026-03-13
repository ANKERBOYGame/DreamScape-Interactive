namespace DreamScape_Interactive.Models
{
    /// <summary>
    /// Represents a registered user in the application.
    /// </summary>
    public class User
    {
        public string Username { get; }
        public string PasswordHash { get; }

        public User(string username, string passwordHash)
        {
            Username = username;
            PasswordHash = passwordHash;
        }
    }
}

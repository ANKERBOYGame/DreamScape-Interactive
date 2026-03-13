using DreamScape_Interactive.Models;

namespace DreamScape_Interactive.Services
{
    /// <summary>
    /// Manages the current user session.
    /// </summary>
    public static class SessionManager
    {
        /// <summary>
        /// The currently logged-in user, or null if no session is active.
        /// </summary>
        public static User? CurrentUser { get; private set; }

        /// <summary>
        /// Returns true when a user is logged in.
        /// </summary>
        public static bool IsLoggedIn => CurrentUser != null;

        /// <summary>
        /// Starts a new session for the given user.
        /// </summary>
        public static void StartSession(User user)
        {
            CurrentUser = user;
        }

        /// <summary>
        /// Ends the current session.
        /// </summary>
        public static void EndSession()
        {
            CurrentUser = null;
        }
    }
}

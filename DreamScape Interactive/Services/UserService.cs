using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using DreamScape_Interactive.Models;

namespace DreamScape_Interactive.Services
{
    /// <summary>
    /// Handles user authentication.
    /// Passwords are stored as PBKDF2-SHA256 hashes with a random salt.
    /// </summary>
    public class UserService
    {
        private const int Iterations = 310_000;
        private const int HashSize = 32; // 256-bit
        private const int SaltSize = 16; // 128-bit

        private readonly Dictionary<string, User> _users =
            new(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Shared application-wide instance.
        /// </summary>
        public static UserService Instance { get; } = new UserService();

        private UserService()
        {
            // Seed two default accounts for demo/testing purposes.
            AddUser("admin", "Admin@123");
            AddUser("player", "Player@123");
        }

        /// <summary>
        /// Adds a user with a securely hashed password to the store.
        /// </summary>
        public void AddUser(string username, string plainPassword)
        {
            var hash = HashPassword(plainPassword);
            _users[username] = new User(username, hash);
        }

        /// <summary>
        /// Validates credentials and returns the matching User, or null on failure.
        /// </summary>
        public User? Authenticate(string username, string plainPassword)
        {
            if (!_users.TryGetValue(username, out var user))
                return null;

            return VerifyPassword(plainPassword, user.PasswordHash) ? user : null;
        }

        private static string HashPassword(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(SaltSize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                Iterations,
                HashAlgorithmName.SHA256,
                HashSize);

            return $"{Iterations}:{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
        }

        private static bool VerifyPassword(string password, string storedHash)
        {
            var parts = storedHash.Split(':');
            if (parts.Length != 3) return false;
            if (!int.TryParse(parts[0], out var iterations)) return false;

            var salt = Convert.FromBase64String(parts[1]);
            var expectedHash = Convert.FromBase64String(parts[2]);

            var actualHash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                iterations,
                HashAlgorithmName.SHA256,
                expectedHash.Length);

            // Constant-time comparison to prevent timing-based side-channel attacks.
            return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
        }
    }
}

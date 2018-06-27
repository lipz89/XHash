using System;
using System.Security.Cryptography;

namespace XHashGen
{
    /// <summary>
    /// Salted password hashing with PBKDF2-SHA1.
    /// Author: havoc AT defuse.ca
    /// www: http://crackstation.net/hashing-security.htm
    /// Compatibility: .NET 3.0 and later.
    /// </summary>
    public static class XHash
    {
        // The following constants may be changed without breaking existing hashes.
        private const int SALT_BYTE_SIZE = 24;
        private const int HASH_BYTE_SIZE = 24;
        private const int PBKDF2_AUTHORIZE_ITERATIONS = 50;

        /// <summary>
        /// Creates a salted PBKDF2 hash of the password.
        /// </summary>
        /// <param name="tick"></param>
        /// <param name="publicKey"></param>
        /// <returns>The hash of the password.</returns>
        public static string CreateAuthorize(string tick, string publicKey)
        {
            if (string.IsNullOrWhiteSpace(tick) || string.IsNullOrWhiteSpace(publicKey))
            {
                return null;
            }
            // Generate a random salt
            RNGCryptoServiceProvider csprng = new RNGCryptoServiceProvider();
            byte[] salt = new byte[SALT_BYTE_SIZE];
            csprng.GetBytes(salt);

            var input = tick + publicKey;
            // Hash the password and encode the parameters
            byte[] hash = Pbkdf2(input, salt, PBKDF2_AUTHORIZE_ITERATIONS, HASH_BYTE_SIZE);
            return Convert.ToBase64String(salt) + Convert.ToBase64String(hash);
        }

        /// <summary>
        /// Computes the PBKDF2-SHA1 hash of a password.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <param name="salt">The salt.</param>
        /// <param name="iterations">The PBKDF2 iteration count.</param>
        /// <param name="outputBytes">The length of the hash to generate, in bytes.</param>
        /// <returns>A hash of the password.</returns>
        private static byte[] Pbkdf2(string password, byte[] salt, int iterations, int outputBytes)
        {
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt);
            pbkdf2.IterationCount = iterations;
            return pbkdf2.GetBytes(outputBytes);
        }
    }
}
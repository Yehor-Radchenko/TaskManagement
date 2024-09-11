namespace TaskManagement.BLL.Services;

using System.Security.Cryptography;
using System.Text;

/// <summary>
/// Provides static methods for securely hashing and verifying passwords.
/// This class adheres to best practices for password security by implementing salting and using a secure hashing algorithm (SHA256).
/// </summary>
public static class PasswordHasherService
{
    /// <summary>
    /// The size of the salt to be used in bytes (default: 16).
    /// A larger salt size increases security but also increases storage requirements.
    /// </summary>
    private const int SaltSize = 16;

    /// <summary>
    /// The size of the hashed password in bytes (default: 32, which is the output size of SHA256).
    /// </summary>
    private const int HashSize = 32;

    /// <summary>
    /// Generates a secure hash of the provided password for storage.
    /// This method uses a random salt to prevent pre-computed rainbow table attacks.
    /// The hashed password is returned as a Base64 encoded string for easier storage and transfer.
    /// </summary>
    /// <param name="password">The password to hash.</param>
    /// <returns>The hashed password as a Base64 string.</returns>
    /// <exception cref="ArgumentException">Throws an exception if the password is null, empty, or whitespace.</exception>
    public static string HashPassword(string password)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(password);

        byte[] salt;
        using (var randomNumberGenerator = RandomNumberGenerator.Create())
        {
            salt = new byte[SaltSize];
            randomNumberGenerator.GetBytes(salt);
        }

        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
        byte[] saltedPassword = new byte[passwordBytes.Length + salt.Length];

        Array.Copy(passwordBytes, saltedPassword, passwordBytes.Length);
        Array.Copy(salt, 0, saltedPassword, passwordBytes.Length, salt.Length);

        byte[] hash = SHA256.HashData(saltedPassword);
        byte[] hashBytes = new byte[SaltSize + HashSize];
        Array.Copy(salt, 0, hashBytes, 0, SaltSize);
        Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

        return Convert.ToBase64String(hashBytes);
    }

    /// <summary>
    /// Verifies if the provided password matches the stored hashed password.
    /// This method retrieves the salt from the stored hash (encoded in Base64) and combines it with the newly provided password before hashing it again.
    /// The newly generated hash is then compared to the stored hash for verification.
    /// </summary>
    /// <param name="password">The password to verify.</param>
    /// <param name="storedHash">The stored hash of the password as a Base64 string.</param>
    /// <returns>True if the password matches the stored hash, false otherwise.</returns>
    /// <exception cref="ArgumentException">Throws an exception if either the password or the stored hash is null, empty, or whitespace.</exception>
    public static bool VerifyPassword(string password, string storedHash)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(password);
        ArgumentException.ThrowIfNullOrWhiteSpace(storedHash);

        byte[] hashBytes = Convert.FromBase64String(storedHash);
        byte[] salt = new byte[SaltSize];
        Array.Copy(hashBytes, 0, salt, 0, SaltSize);
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
        byte[] saltedPassword = new byte[passwordBytes.Length + salt.Length];

        Array.Copy(passwordBytes, saltedPassword, passwordBytes.Length);
        Array.Copy(salt, 0, saltedPassword, passwordBytes.Length, salt.Length);

        byte[] hash = SHA256.HashData(saltedPassword);

        for (int i = 0; i < HashSize; i++)
        {
            if (hashBytes[i + SaltSize] != hash[i])
            {
                return false;
            }
        }

        return true;
    }
}

using System;
using System.Security.Cryptography;
using System.Text;

public static class PasswordHasher
{
    public static string HashPassword(string password)
    {
        // Simple SHA256 hash (not salted)
        using (SHA256 sha = SHA256.Create())
        {
            byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }

    public static bool VerifyPassword(string password, string storedHash)
    {
        string hashOfInput = HashPassword(password);
        return hashOfInput == storedHash;
    }
}
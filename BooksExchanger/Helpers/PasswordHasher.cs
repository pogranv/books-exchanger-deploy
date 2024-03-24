using System.Security.Cryptography;
using System.Text;

namespace BooksExchanger.Models;

/// <summary>
/// Класс для хеширования паролей.
/// </summary>
public class PasswordHasher
{
    /// <summary>
    /// Хеширует пароль.
    /// </summary>
    /// <param name="password">Пароль.</param>
    /// <returns>Захешированный пароль.</returns>
    public static string HashPassword(string password)
    {
        var bytes = Encoding.UTF8.GetBytes(password);
        using (var hash = SHA256.Create())
        {
            var hashedBytes = hash.ComputeHash(bytes);
            return Convert.ToBase64String(hashedBytes);
        }
    }
}
using System.Security.Cryptography;
using System.Text;

namespace BooksExchanger.Models;

/// <summary>
/// Класс для хеширования паролей.
/// </summary>
public class PasswordHasher
{
    private const int SaltSize = 16; // Размер соли в байтах
    private const int KeySize = 32; // Размер ключа (хеша) в байтах
    private const int Iterations = 10000; // Количество итераций
    public static string BuildSalt()
    {
        using (var rng = new RNGCryptoServiceProvider())
        {
            byte[] saltBytes = new byte[SaltSize];
            rng.GetBytes(saltBytes); // Генерируем соль
            return Convert.ToBase64String(saltBytes);
        }
    }
    /// <summary>
    /// Хеширует пароль.
    /// </summary>
    /// <param name="password">Пароль.</param>
    /// <returns>Захешированный пароль.</returns>
    public static string HashPassword(string password, string salt)
    {
        var saltBytes = Encoding.UTF8.GetBytes(salt);
        using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, Iterations))
        {
            byte[] hashBytes = pbkdf2.GetBytes(KeySize);
            
            byte[] hashWithSaltBytes = new byte[SaltSize + KeySize]; // Объединяем соль и хеш
            Array.Copy(saltBytes, 0, hashWithSaltBytes, 0, SaltSize);
            Array.Copy(hashBytes, 0, hashWithSaltBytes, SaltSize, KeySize);
            
            return Convert.ToBase64String(hashWithSaltBytes);
        }
    }
}
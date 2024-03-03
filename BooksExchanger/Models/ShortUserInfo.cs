namespace BooksExchanger.Models;

/// <summary>
/// Краткая информация о пользователе.
/// </summary>
public class ShortUserInfo
{
    /// <summary>
    /// email пользователя.
    /// </summary>
    public string Email { get; set; }
        
    /// <summary>
    /// Пароль пользователя.
    /// </summary>
    public string Password { get; set; }
        
    /// <summary>
    /// Имя пользователя.
    /// </summary>
    public string Name { get; set; }
}
namespace BooksExchanger.Models;

/// <summary>
/// Модель пользовтаеля.
/// </summary>
public class User
{
    /// <summary>
    /// id пользователя.
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// email пользователя.
    /// </summary>
    public string Email { get; set; }
    
    /// <summary>
    /// Соль пароля пользователя.
    /// </summary>
    public string Salt { get; set; }

    /// <summary>
    /// Пароль пользователя.
    /// </summary>
    public string Password { get; set; }
    
    /// <summary>
    /// Имя пользователя.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Роль пользоватля.
    /// </summary>
    public UserRole Role { get; set; }
}
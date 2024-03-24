namespace BooksExchanger.Models;

/// <summary>
/// Модель авторизированного пользорвателя.
/// </summary>
public class AuthUser
{
    /// <summary>
    /// Id пользователя.
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// Роль пользователя.
    /// </summary>
    public string Role { get; set; }
}
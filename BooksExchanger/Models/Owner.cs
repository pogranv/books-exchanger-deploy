namespace BooksExchanger.Models;

/// <summary>
/// Модель автора.
/// </summary>
public class Owner
{
    /// <summary>
    /// id пользователя.
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// Имя пользователя.
    /// </summary>
    public string Name { get; set; }
}
namespace BooksExchanger.Controllers.Specs.Common;

/// <summary>
/// Модель владельца объявления.
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
namespace BooksExchanger.Models;

/// <summary>
/// Модель жанра.
/// </summary>
public class Genre
{
    /// <summary>
    /// id жанра.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Наименование жанра.
    /// </summary>
    public string Name { get; set; }
}
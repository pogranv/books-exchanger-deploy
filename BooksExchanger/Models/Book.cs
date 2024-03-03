namespace BooksExchanger.Models;

/// <summary>
/// Модель книги.
/// </summary>
public class Book
{
    /// <summary>
    /// id книги.
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// Название книги.
    /// </summary>
    public string Title { get; set; }
    
    /// <summary>
    /// Жанр книги.
    /// </summary>
    public Genre Genre { get; set; }
    
    /// <summary>
    /// Информаиця об авторах книги.
    /// </summary>
    public List<Author> Authors { get; set; }
}
namespace BooksExchanger.Entities;

/// <summary>
/// Объект жанра в БД.
/// </summary>
public partial class Genre
{
    /// <summary>
    /// id жанра.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Название жанра.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Связанные книги.
    /// </summary>
    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}

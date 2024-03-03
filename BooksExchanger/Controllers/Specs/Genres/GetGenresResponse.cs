namespace BooksExchanger.Controllers.Specs.Genres;

/// <summary>
/// Модель ответа получения жанров.
/// </summary>
public class GetGenresResponse
{
    /// <summary>
    /// Информация о жанрах.
    /// </summary>
    public List<Common.Genre> Genres { get; set; }
}
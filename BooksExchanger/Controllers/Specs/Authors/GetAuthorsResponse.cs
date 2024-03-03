namespace BooksExchanger.Controllers.Specs.Authors;

/// <summary>
/// Модель ответа для получения авторов.
/// </summary>
public class GetAuthorsResponse
{
    /// <summary>
    /// Список авторов.
    /// </summary>
    public List<Common.Author> Authors { get; set; }
}
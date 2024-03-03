using BooksExchanger.Controllers.Specs.Common;

namespace BooksExchanger.Controllers.Specs.Books;

/// <summary>
/// Модель запроса для получения книг.
/// </summary>
public class GetBooksResponse
{
    /// <summary>
    /// Информация о запрашиваемых книгах.
    /// </summary>
    public List<Book> Books { get; set; } = new();
}
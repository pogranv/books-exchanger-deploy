using BooksExchanger.Controllers.Specs.Common;

namespace BooksExchanger.Controllers.Specs.Offers;

/// <summary>
/// Модель ответа получения книжных подборок.
/// </summary>
public class GetOffersSelectionsResponse
{
    /// <summary>
    /// Подборки книг.
    /// </summary>
    public List<Selection> Selections { get; set; } = new ();
}
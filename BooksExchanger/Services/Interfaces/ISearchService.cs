using BooksExchanger.Models;

namespace BooksExchanger.Services.Interfaces;

/// <summary>
/// Определяет интерфейс сервиса для поиска предложений.
/// </summary>
public interface ISearchService
{
    /// <summary>
    /// Ищет предложения по названию книги.
    /// </summary>
    /// <param name="title">Название книги.</param>
    /// <returns>Коллекцию предложений, соответствующих заданному названию книги.</returns>
    IEnumerable<Offer> SearchOffersByTitle(string title);
    
    /// <summary>
    /// Ищет предложения по автору книги.
    /// </summary>
    /// <param name="author">Имя автора книги.</param>
    /// <returns>Коллекцию предложений, соответствующих указанному автору.</returns>
    IEnumerable<Offer> SearchOffersByAuthor(string author);
}
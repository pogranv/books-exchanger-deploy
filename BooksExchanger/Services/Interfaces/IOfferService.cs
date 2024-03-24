using BooksExchanger.Models;

namespace BooksExchanger.Services.Interfaces;

/// <summary>
/// Определяет интерфейс для службы управления предложениями обмена книгами.
/// </summary>
public interface IOfferService
{
    /// <summary>
    /// Получает информацию о предложении по его идентификатору.
    /// </summary>
    /// <param name="offerId">Идентификатор предложения.</param>
    /// <returns>Данные предложения.</returns>
    Offer GetOffer(Guid offerId);

    /// <summary>
    /// Проверяет, размечено ли предложение как избранное у пользователя.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="offerId">Идентификатор предложения.</param>
    /// <returns>true, если предложение находится в избранных у пользователя; иначе false.</returns>
    public bool IsOfferFavoriteForUser(long userId, Guid offerId);

    /// <summary>
    /// Получает список предложений на основе заданных критериев.
    /// </summary>
    /// <param name="genreId">Идентификатор жанра.</param>
    /// <param name="city">Город.</param>
    /// <param name="userId">Идентификатор пользователя, предложения которого требуется исключить.</param>
    /// <param name="notUserId">Идентификатор пользователя, чьи предложения требуется исключить из поиска.</param>
    /// <returns>Коллекция подходящих предложений.</returns>
    public IEnumerable<Offer> GetOffers(int? genreId = null, string? city = null, long? userId = null,
        long? notUserId = null);

    /// <summary>
    /// Добавляет предложение в избранное пользователя.
    /// </summary>
    /// <param name="offerId">Идентификатор предложения.</param>
    /// <param name="userId">Идентификатор пользователя.</param>
    public void AddOfferToFavorite(Guid offerId, long userId);
    
    /// <summary>
    /// Удаляет предложение из избранных у пользователя.
    /// </summary>
    /// <param name="offerId">Идентификатор предложения.</param>
    /// <param name="userId">Идентификатор пользователя.</param>
    public void RemoveOfferFromFavorite(Guid offerId, long userId);

    /// <summary>
    /// Удаляет предложение обмена.
    /// </summary>
    /// <param name="offerId">Идентификатор предложения.</param>
    /// <param name="userId">Идентификатор пользователя, совершающего удаление.</param>
    public void RemoveOffer(Guid offerId, long userId);

    /// <summary>
    /// Получает список избранных предложений пользователя.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <returns>Коллекция предложений, помеченных как избранное у пользователя.</returns>
    public IEnumerable<Offer> GetFavoriteOffers(long userId);

    /// <summary>
    /// Получает список рекомендаций для предложений на основе предпочтений пользователя и его местоположения.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="city">Город пользователя.</param>
    /// <returns>Список рекомендаций для предложений.</returns>
    public List<Selection> GetOfferSelections(long? userId, string? city);

    /// <summary>
    /// Ищет предложения по названию книги.
    /// </summary>
    /// <param name="title">Название книги.</param>
    /// <returns>Список предложений, чьи названия соответствуют запросу.</returns>
    public List<Offer> SearchOffersByTitle(string title);
    
    /// <summary>
    /// Ищет предложения по автору книги.
    /// </summary>
    /// <param name="author">Автор книги.</param>
    /// <returns>Список предложений, чьи авторы соответствуют запросу.</returns>
    public List<Offer> SearchOffersByAuthor(string author);
}
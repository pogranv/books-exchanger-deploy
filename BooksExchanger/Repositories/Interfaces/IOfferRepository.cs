using BooksExchanger.Models;

namespace BooksExchanger.Repositories.Interfaces;

/// <summary>
/// Интерфейс хранилища офферов.
/// </summary>
public interface IOfferRepository
{
    /// <summary>
    /// Возвращает отзыв.
    /// </summary>
    /// <param name="offerId">id отзыва.</param>
    /// <returns>Информация об отзыве.</returns>
    public Offer GetOffer(Guid offerId);

    /// <summary>
    /// Проверяет, является ли предложение избранным для пользователя.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="offerId">Идентификатор предложения.</param>
    /// <returns>True, если предложение избранное для пользователя; иначе false.</returns>
    public bool IsFavoriteForUser(long userId, Guid offerId);

    /// <summary>
    /// Возвращает список предложений с учетом указанных функций фильтрации.
    /// </summary>
    /// <param name="genreFilter">Функция фильтрации по жанру.</param>
    /// <param name="cityFilter">Функция фильтрации по городу.</param>
    /// <param name="userFilter">Функция фильтрации по идентификатору пользователя.</param>
    /// <param name="notUserFilter">Функция фильтрации по идентификатору пользователя, исключая указанного.</param>
    /// <returns>Коллекция объектов предложений.</returns>
    public IEnumerable<Offer> GetOffers(Func<int, bool>? genreFilter = null, Func<string, bool>? cityFilter = null,
        Func<long, bool>? userFilter = null, Func<long, bool>? notUserFilter = null);

    /// <summary>
    /// Добавляет предложение в избранное для пользователя.
    /// </summary>
    /// <param name="offerId">Идентификатор предложения.</param>
    /// <param name="userId">Идентификатор пользователя.</param>
    public void AddOfferToFavorite(Guid offerId, long userId);

    /// <summary>
    /// Удаляет предложение из избранного для пользователя.
    /// </summary>
    /// <param name="offerId">Идентификатор предложения.</param>
    /// <param name="userId">Идентификатор пользователя.</param>
    public void RemoveOfferFromFavorite(Guid offerId, long userId);

    /// <summary>
    /// Удаляет предложение.
    /// </summary>
    /// <param name="offerId">Идентификатор предложения.</param>
    public void RemoveOffer(Guid offerId);

    /// <summary>
    /// Проверяет, является ли пользователь владельцем предложения.
    /// </summary>
    /// <param name="offerId">Идентификатор предложения.</param>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <returns>True, если пользователь является владельцем предложения; иначе false.<returns>
    public bool IsUserOwnerOffer(Guid offerId, long userId);

    /// <summary>
    /// Возвращает список избранных предложений для пользователя.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <returns>Коллекция объектов предложений избранных для пользователя.</returns>
    public IEnumerable<Offer> GetFavoriteOffers(long userId);

    /// <summary>
    /// Возвращает список предложений по указанному жанру с учетом функций фильтрации.
    /// </summary>
    /// <param name="genreId">Идентификатор жанра.</param>
    /// <param name="cityFilter">Функция фильтрации по городу.</param>
    /// <param name="notUserFilter">Функция фильтрации по идентификатору пользователя, исключая указанного.</param>
    /// <returns>Коллекция объектов предложений по указанному жанру.</returns>
    public IEnumerable<Offer> GetOffersByGenre(int genreId, Func<string, bool>? cityFilter = null,
        Func<long, bool>? notUserFilter = null);

    /// <summary>
    /// Возвращает список предложений, название которых начинается с указанной строки.
    /// </summary>
    /// <param name="title">Начальная строка названия предложения.</param>
    /// <returns>Коллекция объектов предложений.</returns>
    public IEnumerable<Offer> GetOffersByTitleStart(string title);
    
    /// <summary>
    /// Возвращает список предложений, автор которых начинается с указанной строки.
    /// </summary>
    /// <param name="author">Начальная строка для имени автора предложения.</param>
    /// <returns>Коллекция объектов предложений.</returns>
    public IEnumerable<Offer> GetOffersByAuthorStart(string author);
}
using BooksExchanger.Models;
using BooksExchanger.Repositories.Interfaces;
using BooksExchanger.Services.Exceptions;
using BooksExchanger.Services.Interfaces;

using Selection = BooksExchanger.Models.Selection;

namespace BooksExchanger.Services.Implementations.OfferService;


/// <summary>
/// Сервис для работы с предложениями книг.
/// </summary>
public class OfferService : IOfferService
{
    private IOfferRepository _offerRepository;
    private IGenreRepository _genreRepository;

    /// <summary>
    /// Конструктор сервиса предложений.
    /// </summary>
    /// <param name="offerRepository">Репозиторий предложений.</param>
    /// <param name="genreRepository">Репозиторий жанров.</param>
    public OfferService(IOfferRepository offerRepository, IGenreRepository genreRepository)
    {
        _offerRepository = offerRepository;
        _genreRepository = genreRepository;
    }

    /// <summary>
    /// Получает предложение по идентификатору.
    /// </summary>
    /// <param name="offerId">Идентификатор предложения.</param>
    /// <returns>Объект предложения.</returns>
    public Offer GetOffer(Guid offerId)
    {
        try
        {
            return _offerRepository.GetOffer(offerId);
        }
        catch (Repositories.Exeptions.OfferNotFoundException ex)
        {
            throw new OfferNotFoundException(ex.Message);
        }
    }

    /// <summary>
    /// Проверяет, добавлено ли предложение в избранное у пользователя.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="offerId">Идентификатор предложения.</param>
    /// <returns>True, если предложение в избранном у пользователя. Иначе false.</returns>
    public bool IsOfferFavoriteForUser(long userId, Guid offerId)
    {
        return _offerRepository.IsFavoriteForUser(userId, offerId);
    }

    /// <summary>
    /// Получает список предложений книг в соответствии с указанными фильтрами.
    /// </summary>
    /// <param name="genreId">Идентификатор жанра (опционально).</param>
    /// <param name="city">Город (опционально).</param>
    /// <param name="userId">Идентификатор пользователя, предложения которого нужно получить (опционально).</param>
    /// <param name="notUserId">Идентификатор пользователя, предложения которого нужно исключить (опционально).</param>
    /// <returns>Коллекция предложений книг.</returns>
    public IEnumerable<Offer> GetOffers(int? genreId = null, string? city = null, long? userId = null, long? notUserId = null)
    {
        Func<int, bool>? genreFilter = null;
        if (genreId != null)
        {
            genreFilter = actualGenreId => actualGenreId == genreId;
        }
        Func<string, bool>? cityFilter = null;
        if (city != null)
        {
            cityFilter = actualCity => actualCity.ToLower() == city.ToLower();
        }
        Func<long, bool>? userFilter = null;
        if (userId != null)
        {
            userFilter = actualUserId => actualUserId == userId;
        }
        Func<long, bool>? notUserFilter = null;
        if (notUserId != null)
        {
            notUserFilter = actualUserId => actualUserId != notUserId;
        }

        return _offerRepository.GetOffers(genreFilter, cityFilter, userFilter, notUserFilter);
    }

    /// <summary>
    /// Добавляет предложение в избранное пользователя.
    /// </summary>
    /// <param name="offerId">Идентификатор предложения.</param>
    /// <param name="userId">Идентификатор пользователя.</param>
    public void AddOfferToFavorite(Guid offerId, long userId)
    {
        try
        {
            _offerRepository.AddOfferToFavorite(offerId, userId);
        }
        catch (Repositories.Exeptions.OfferAlreadyFavoriteException ex)
        {
            throw new OfferAlreadyFavoriteException(ex.Message);
        }
    }

    /// <summary>
    /// Удаляет предложение из избранного пользователя.
    /// </summary>
    /// <param name="offerId">Идентификатор предложения.</param>
    /// <param name="userId">Идентификатор пользователя.</param>
    public void RemoveOfferFromFavorite(Guid offerId, long userId)
    {
        try
        {
            _offerRepository.RemoveOfferFromFavorite(offerId, userId);
        }
        catch (Repositories.Exeptions.OfferAlreadyNotInFavoritesException ex)
        {
            throw new OfferAlreadyNotInFavoritesException(ex.Message);
        }
    }

    /// <summary>
    /// Удаляет предложение книги.
    /// </summary>
    /// <param name="offerId">Идентификатор предложения.</param>
    /// <param name="userId">Идентификатор пользователя, владельца предложения.</param>
    public void RemoveOffer(Guid offerId, long userId)
    {
        try
        {
            if (!_offerRepository.IsUserOwnerOffer(offerId, userId))
            {
                throw new UserNotOfferOwnerException("Пользователь не является владельцем объявления");
            }
            _offerRepository.RemoveOffer(offerId);
        }
        catch (Repositories.Exeptions.OfferNotFoundException ex)
        {
            throw new OfferNotFoundException(ex.Message);
        }
    }

    /// <summary>
    /// Получает список избранных предложений пользователя.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <returns>Коллекция избранных предложений.</returns>
    public IEnumerable<Offer> GetFavoriteOffers(long userId)
    {
        return _offerRepository.GetFavoriteOffers(userId);
    }

    /// <summary>
    /// Получение подборок офферов.
    /// </summary>
    /// <param name="userId">id пользователя.</param>
    /// <param name="city">Город.</param>
    /// <returns></returns>
    public List<Selection> GetOfferSelections(long? userId, string? city)
    {
        Func<string, bool>? cityFilter = null;
        if (city != null)
        {
            cityFilter = actualCity => actualCity == city;
        }
        Func<long, bool>? notUserFilter = null;
        if (userId != null)
        {
            notUserFilter = actualUserId => actualUserId != userId;
        }

        var result = new List<Selection>();
        var genres = _genreRepository.GetGenres();
        
        foreach (var genre in genres)
        {
            var offers = _offerRepository.GetOffersByGenre(genre.Id, cityFilter, notUserFilter).ToList();
            if (offers.Count == 0)
            {
                continue;
            }
            result.Add(new Selection
            {
                Title = genre.Name,
                Offers = offers
            });
        }

        return result;
    }

    /// <summary>
    /// Поиск офферов по названию.
    /// </summary>
    /// <param name="title">Название оффера.</param>
    /// <returns>Спискок офферов.</returns>
    public List<Offer> SearchOffersByTitle(string title)
    {
        return _offerRepository.GetOffersByTitleStart(title).ToList();
    }

    /// <summary>
    /// Поиск офферов по автору.
    /// </summary>
    /// <param name="author">Имя автора.</param>
    /// <returns>Спискок офферов.</returns>
    public List<Offer> SearchOffersByAuthor(string author)
    {
        return _offerRepository.GetOffersByAuthorStart(author).ToList();
    }
}
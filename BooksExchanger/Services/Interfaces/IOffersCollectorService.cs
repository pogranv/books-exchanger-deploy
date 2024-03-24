using BooksExchanger.Models;

namespace BooksExchanger.Services.Interfaces;

/// <summary>
/// Интерфейс сервиса для управления предложениями книг.
/// </summary>
public interface IOffersCollectorService
{
    /// <summary>
    /// Создает новое предложение книги.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя, создающего предложение.</param>
    /// <param name="title">Название книги.</param>
    /// <param name="authors">Авторы книги.</param>
    /// <param name="city">Город, в котором находится книга.</param>
    /// <param name="description">Описание книги (необязательно).</param>
    /// <param name="image">Изображение обложки книги (необязательно).</param>
    /// <param name="price">Цена книги (необязательно).</param>
    /// <returns>Идентификатор созданного предложения.</returns>
    public Task<Guid> CreateOffer(long userId, string title, string authors, string city, string? description,
        IFormFile? image, decimal? price);

    /// <summary>
    /// Отклоняет предложение книги по указанной причине.
    /// </summary>
    /// <param name="offerId">Идентификатор предложения.</param>
    /// <param name="reason">Причина отклонения предложения.</param>
    /// <returns>Идентификатор отклоненного предложения.</returns>
    public Guid RejectOffer(Guid offerId, string reason);

    /// <summary>
    /// Одобряет предложение книги и связывает его с существующей книгой.
    /// </summary>
    /// <param name="offerId">Идентификатор предложения.</param>
    /// <param name="linkedBookId">Идентификатор книги, с которой будет связано предложение.</param>
    /// <returns>Идентификатор подтвержденного предложения.</returns>
    public Guid ApproveOfferAndGetId(Guid offerId, long linkedBookId);

    /// <summary>
    /// Удаляет предложение книги.
    /// </summary>
    /// <param name="offerId">Идентификатор предложения.</param>
    /// <param name="userId">Идентификатор пользователя, который является владельцем предложения.</param>
    public void RemoveOffer(Guid offerId, long userId);

    /// <summary>
    /// Получает предложения книг в соответствии с указанными критериями.
    /// </summary>
    /// <param name="needStatuses">Набор статусов модерации, по которым выполняется фильтрация предложений.</param>
    /// <param name="offerId">Идентификатор конкретного предложения (необязательно).</param>
    /// <param name="userId">Идентификатор пользователя, чьи предложения нужно получить (необязательно).</param>
    /// <returns>Коллекция предложений книг.</returns>
    public IEnumerable<OfferCollector> GetOffers(HashSet<ModerationStatus> needStatuses, Guid? offerId = null,
        long? userId = null);
}
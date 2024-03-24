using BooksExchanger.Models;

namespace BooksExchanger.Repositories.Interfaces;

/// <summary>
/// Интерфейс репозитория для управления предложениями книг.
/// </summary>
public interface IOffersCollectorRepository
{
    /// <summary>
    /// Добавляет новое предложение книги.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя, создавшего предложение.</param>
    /// <param name="title">Название книги.</param>
    /// <param name="authors">Авторы книги.</param>
    /// <param name="city">Город, в котором находится книга.</param>
    /// <param name="description">Описание книги (необязательно).</param>
    /// <param name="imageLink">Ссылка на изображение обложки книги (необязательно).</param>
    /// <param name="price">Цена книги (необязательно).</param>
    /// <returns>Идентификатор созданного предложения.</returns>
    public Guid AddOffer(long userId, string title, string authors, string city, string? description, string? imageLink,
        decimal? price);

    /// <summary>
    /// Устанавливает для предложения статус "отклонено" с указанием причины.
    /// </summary>
    /// <param name="offerId">Идентификатор предложения.</param>
    /// <param name="reason">Причина отклонения предложения.</param>
    /// <returns>Идентификатор предложения.</returns>
    public Guid SetRejectAndReason(Guid offerId, string reason);

    /// <summary>
    /// Перемещает предложение в подтвержденные с привязкой к существующей книге.
    /// </summary>
    /// <param name="offerId">Идентификатор предложения.</param>
    /// <param name="linkedBookId">Идентификатор связанной книги.</param>
    /// <returns>Идентификатор нового предложения в списке подтвержденных.</returns>
    public Guid MoveOfferToApprovedOffers(Guid offerId, long linkedBookId);

    /// <summary>
    /// Проверяет, принадлежит ли предложение указанному пользователю.
    /// </summary>
    /// <param name="offerId">Идентификатор предложения.</param>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <returns>True, если пользователь является владельцем предложения; иначе false.</returns>
    public bool CheckUserOwner(Guid offerId, long userId);

    /// <summary>
    /// Удаляет предложение.
    /// </summary>
    /// <param name="offerId">Идентификатор предложения для удаления.</param>
    public void RemoveOffer(Guid offerId);

    /// <summary>
    /// Получает информацию о предложении по его идентификатору.
    /// </summary>
    /// <param name="offerId">Идентификатор предложения.</param>
    /// <returns>Предложение с информацией или null, если предложение не найдено.</returns>
    public OfferCollector GetOffer(Guid? offerId);

    /// <summary>
    /// Получает список предложений по заданным статусам модерации и, опционально, идентификатору пользователя.
    /// </summary>
    /// <param name="moderationStatusSet">Набор статусов модерации.</param>
    /// <param name="userId">Идентификатор пользователя (необязательно).</param>
    /// <returns>Список предложений.</returns>
    public IEnumerable<OfferCollector> GetOffers(HashSet<Models.ModerationStatus> moderationStatusSet,
        long? userId = null);
}
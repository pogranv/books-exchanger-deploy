using BooksExchanger.Models;
using BooksExchanger.Repositories.Implementations;
using BooksExchanger.Repositories.Interfaces;
using BooksExchanger.Services.Exceptions;
using BooksExchanger.Services.Interfaces;
using BookNotFoundException = BooksExchanger.Services.Exceptions.BookNotFoundException;

namespace BooksExchanger.Services.Implementations.OffersCollectorService;

/// <summary>
/// Сервис офферов для модерации.
/// </summary>
public class OffersCollectorService : IOffersCollectorService
{
    private IImageStorageService _imageStorageService;
    private IOffersCollectorRepository _offersCollectorRepository;

    /// <summary>
    /// Инициализирует новый экземпляр службы сбора предложений.
    /// </summary>
    /// <param name="imageStorageService">Служба хранения изображений.</param>
    /// <param name="offersCollectorRepository">Репозиторий коллектора предложений.</param>
    public OffersCollectorService(IImageStorageService imageStorageService, IOffersCollectorRepository offersCollectorRepository)
    {
        _imageStorageService = imageStorageService;
        _offersCollectorRepository = offersCollectorRepository;
    }
    
    /// <summary>
    /// Создает предложение об обмене книгой.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="title">Название книги.</param>
    /// <param name="authors">Авторы книги.</param>
    /// <param name="city">Город, в котором предлагается книга.</param>
    /// <param name="description">Описание книги.</param>
    /// <param name="image">Фотография обложки книги.</param>
    /// <param name="price">Цена предложения.</param>
    /// <returns>Идентификатор созданного предложения.</returns>
    public async Task<Guid> CreateOffer(long userId, string title, string authors, string city, string? description, IFormFile? image, decimal? price)
    {
        string? imageLink = null;
        if (image != null)
        {
            imageLink = await _imageStorageService.UploadImageAndGetLink(image);
        }

        return _offersCollectorRepository.AddOffer(userId, title, authors, city, description, imageLink, price);
    }

    /// <summary>
    /// Отклоняет предложение с указанием причины.
    /// </summary>
    /// <param name="offerId">Идентификатор предложения.</param>
    /// <param name="reason">Причина отклонения.</param>
    /// <returns>Идентификатор отклоненного предложения.</returns>
    public Guid RejectOffer(Guid offerId, string reason)
    {
        try
        {
            return _offersCollectorRepository.SetRejectAndReason(offerId, reason);
        }
        catch (Repositories.Exeptions.OfferNotFoundException ex)
        {
            throw new Services.Exceptions.OfferNotFoundException(ex.Message);
        }
    }

    /// <summary>
    /// Подтверждает предложение и связывает его с идентификатором книги.
    /// </summary>
    /// <param name="offerId">Идентификатор предложения.</param>
    /// <param name="linkedBookId">Идентификатор связанной книги.</param>
    /// <returns>Идентификатор подтвержденного предложения.</returns>
    public Guid ApproveOfferAndGetId(Guid offerId, long linkedBookId)
    {
        try
        {
            return _offersCollectorRepository.MoveOfferToApprovedOffers(offerId, linkedBookId);
        }
        catch (Repositories.Exeptions.OfferAlreadyApprovedException ex)
        {
            throw new Exceptions.OfferAlreadyApprovedException(ex.Message);
        }
        catch (OfferNotFoundException ex)
        {
            throw new Exceptions.OfferNotFoundException(ex.Message);
        }
        catch (Repositories.Exeptions.BookNotFoundException ex)
        {
            throw new BookNotFoundException(ex.Message);
        }
    }

    /// <summary>
    /// Удаляет предложение.
    /// </summary>
    /// <param name="offerId">Идентификатор предложения.</param>
    /// <param name="userId">Идентификатор пользователя, который удаляет предложение.</param>
    public void RemoveOffer(Guid offerId, long userId)
    {
        try
        {
            if (!_offersCollectorRepository.CheckUserOwner(offerId, userId))
            {
                throw new UserNotOfferOwnerException("Пользователь не является владельцем объявления");
            }
            _offersCollectorRepository.RemoveOffer(offerId);
        }
        catch (Repositories.Exeptions.OfferNotFoundException ex)
        {
            throw new Exceptions.OfferNotFoundException(ex.Message);
        }
    }

    /// <summary>
    /// Получает список предложений по заданным критериям.
    /// </summary>
    /// <param name="needStatuses">Статусы предложений, которые необходимо получить.</param>
    /// <param name="offerId">Идентификатор конкретного предложения.</param>
    /// <param name="userId">Идентификатор пользователя, чьи предложения требуется получить.</param>
    /// <returns>Коллекция предложений, удовлетворяющих заданным критериям.</returns>
    public IEnumerable<OfferCollector> GetOffers(HashSet<ModerationStatus> needStatuses, Guid? offerId = null, long? userId = null)
    {
        if (offerId != null)
        {
            return new List<OfferCollector> { _offersCollectorRepository.GetOffer(offerId.Value)} ;
        }

        return _offersCollectorRepository.GetOffers(needStatuses);
    }
}
using BooksExchanger.Models;
using BooksExchanger.Repositories.Implementations;
using BooksExchanger.Repositories.Interfaces;
using BooksExchanger.Services.Exceptions;
using BooksExchanger.Services.Interfaces;
using BookNotFoundException = BooksExchanger.Services.Exceptions.BookNotFoundException;

namespace BooksExchanger.Services.Implementations.OffersCollectorService;

public class OffersCollectorService : IOffersCollectorService
{
    private IImageStorageService _imageStorageService;
    private IOffersCollectorRepository _offersCollectorRepository;
    private ResponseMapper _responseMapper;

    public OffersCollectorService(IImageStorageService imageStorageService, IOffersCollectorRepository offersCollectorRepository)
    {
        _imageStorageService = imageStorageService;
        _offersCollectorRepository = offersCollectorRepository;
        _responseMapper = new();
    }
    
    public async Task<Guid> CreateOffer(long userId, string title, string authors, string city, string? description, IFormFile? image, decimal? price)
    {
        // var authUser = (AuthUser)ControllerContext.HttpContext.Items["User"];
        string? imageLink = null;
        if (image != null)
        {
            imageLink = await _imageStorageService.UploadImageAndGetLink(image);
        }

        return _offersCollectorRepository.AddOffer(userId, title, authors, city, description, imageLink, price);
    }

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

    public IEnumerable<OfferCollector> GetOffers(HashSet<ModerationStatus> needStatuses, Guid? offerId = null, long? userId = null)
    {
        if (offerId != null)
        {
            return new List<OfferCollector> { _offersCollectorRepository.GetOffer(offerId.Value)} ;
        }

        return _offersCollectorRepository.GetOffers(needStatuses);
    }
}
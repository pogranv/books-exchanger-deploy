using BooksExchanger.Models;

namespace BooksExchanger.Services.Interfaces;

public interface IOffersCollectorService
{
    public Task<Guid> CreateOffer(long userId, string title, string authors, string city, string? description,
        IFormFile? image, decimal? price);

    public Guid RejectOffer(Guid offerId, string reason);

    public Guid ApproveOfferAndGetId(Guid offerId, long linkedBookId);

    public void RemoveOffer(Guid offerId, long userId);

    public IEnumerable<OfferCollector> GetOffers(HashSet<ModerationStatus> needStatuses, Guid? offerId = null,
        long? userId = null);
}
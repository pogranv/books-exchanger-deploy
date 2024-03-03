using BooksExchanger.Models;

namespace BooksExchanger.Repositories.Interfaces;

public interface IOffersCollectorRepository
{
    public Guid AddOffer(long userId, string title, string authors, string city, string? description, string? imageLink,
        decimal? price);

    public Guid SetRejectAndReason(Guid offerId, string reason);

    public Guid MoveOfferToApprovedOffers(Guid offerId, long linkedBookId);

    public bool CheckUserOwner(Guid offerId, long userId);

    public void RemoveOffer(Guid offerId);

    public OfferCollector GetOffer(Guid? offerId);

    public IEnumerable<OfferCollector> GetOffers(HashSet<Models.ModerationStatus> moderationStatusSet,
        long? userId = null);
}
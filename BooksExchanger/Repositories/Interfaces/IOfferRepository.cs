using BooksExchanger.Models;

namespace BooksExchanger.Repositories.Interfaces;

public interface IOfferRepository
{
    public Offer GetOffer(Guid offerId);

    public bool IsFavoriteForUser(long userId, Guid offerId);

    public IEnumerable<Offer> GetOffers(Func<int, bool>? genreFilter = null, Func<string, bool>? cityFilter = null,
        Func<long, bool>? userFilter = null, Func<long, bool>? notUserFilter = null);

    public void AddOfferToFavorite(Guid offerId, long userId);

    public void RemoveOfferFromFavorite(Guid offerId, long userId);

    public void RemoveOffer(Guid offerId);

    public bool IsUserOwnerOffer(Guid offerId, long userId);

    public IEnumerable<Offer> GetFavoriteOffers(long userId);

    public IEnumerable<Offer> GetOffersByGenre(int genreId, Func<string, bool>? cityFilter = null,
        Func<long, bool>? notUserFilter = null);

    public IEnumerable<Offer> GetOffersByTitleStart(string title);
    
    
    public IEnumerable<Offer> GetOffersByAuthorStart(string author);
}
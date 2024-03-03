using BooksExchanger.Models;

namespace BooksExchanger.Services.Interfaces;

public interface IOfferService
{
    Models.Offer GetOffer(Guid offerId);

    public bool IsOfferFavoriteForUser(long userId, Guid offerId);

    public IEnumerable<Offer> GetOffers(int? genreId = null, string? city = null, long? userId = null,
        long? notUserId = null);

    public void AddOfferToFavorite(Guid offerId, long userId);
    

    public void RemoveOfferFromFavorite(Guid offerId, long userId);

    public void RemoveOffer(Guid offerId, long userId);

    public IEnumerable<Offer> GetFavoriteOffers(long userId);

    public List<Selection> GetOfferSelections(long? userId, string? city);

    public List<Offer> SearchOffersByTitle(string title);
    
    public List<Offer> SearchOffersByAuthor(string author);
}
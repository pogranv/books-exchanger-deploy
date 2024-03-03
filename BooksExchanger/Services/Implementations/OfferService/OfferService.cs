using BooksExchanger.Context;
using BooksExchanger.Controllers.Specs.Offers;
using BooksExchanger.Models;
using BooksExchanger.Repositories.Interfaces;
using BooksExchanger.Services.Exceptions;
using BooksExchanger.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Selection = BooksExchanger.Models.Selection;

namespace BooksExchanger.Services.Implementations.OfferService;

public class OfferService : IOfferService
{
    private IOfferRepository _offerRepository;
    private IGenreRepository _genreRepository;

    public OfferService(IOfferRepository offerRepository, IGenreRepository genreRepository)
    {
        _offerRepository = offerRepository;
        _genreRepository = genreRepository;
    }

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

    public bool IsOfferFavoriteForUser(long userId, Guid offerId)
    {
        return _offerRepository.IsFavoriteForUser(userId, offerId);
    }

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

    public IEnumerable<Offer> GetFavoriteOffers(long userId)
    {
        return _offerRepository.GetFavoriteOffers(userId);
    }

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

    public List<Offer> SearchOffersByTitle(string title)
    {
        return _offerRepository.GetOffersByTitleStart(title).ToList();
    }

    public List<Offer> SearchOffersByAuthor(string author)
    {
        return _offerRepository.GetOffersByAuthorStart(author).ToList();
    }
}
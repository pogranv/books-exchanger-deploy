using BooksExchanger.Context;
using BooksExchanger.Controllers.Specs;
using BooksExchanger.Controllers.Specs.Offers;
using BooksExchanger.Models;
using BooksExchanger.Repositories.Exeptions;
using BooksExchanger.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace BooksExchanger.Repositories.Implementations;



public class OfferRepository : IOfferRepository
{
    private ResponseMapper _responseMapper;

    public OfferRepository()
    {
        _responseMapper = new();
    }

    public Offer GetOffer(Guid offerId)
    {
        using (DbCtx db = new DbCtx())
        {
            var offer = db.Offers
                .Include(offer => offer.Book)
                .Include(offer => offer.Book.Authors)
                .Include(offer => offer.Book.Genre)
                .Include(offer => offer.Owner)
                .FirstOrDefault(offer => offer.Id == offerId);

            if (offer == null)
            {
                throw new OfferNotFoundException($"Оффера с id={offerId} не найдено");
            }

            return _responseMapper.MapOffer(offer);
        }
    }

    public bool IsFavoriteForUser(long userId, Guid offerId)
    {
        using (DbCtx db = new DbCtx())
        {

            var user = db.Users.Include(user => user.OffersNavigation).FirstOrDefault(user => user.Id == userId);
            return user.OffersNavigation.FirstOrDefault(offer => offer.Id == offerId) != null;
        }
    }

    public IEnumerable<Offer> GetOffers(Func<int, bool>? genreFilter = null, Func<string, bool>? cityFilter = null,
        Func<long, bool>? userFilter = null, Func<long, bool>? notUserFilter = null)
    {
        var filter = (Entities.Offer offer) =>
        {
            bool isMatch = true;
            if (genreFilter != null)
            {
                isMatch = isMatch && genreFilter(offer.Book.Genre.Id);
            }

            if (cityFilter != null)
            {
                isMatch = isMatch && cityFilter(offer.City);
            }

            if (userFilter != null)
            {
                isMatch = isMatch && userFilter(offer.Owner.Id);
            }

            if (notUserFilter != null)
            {
                isMatch = isMatch && notUserFilter(offer.Owner.Id);
            }

            return isMatch;
        };
        using (DbCtx db = new DbCtx())
        {
            var offers = db.Offers
                .Include(offer => offer.Book)
                .Include(offer => offer.Book.Authors)
                .Include(offer => offer.Book.Genre)
                .Include(offer => offer.Owner)
                .AsEnumerable()
                .Where(filter)
                .ToList()
                .ConvertAll(_responseMapper.MapOffer);
            return offers;
        }
    }
    
    public IEnumerable<Offer> GetOffersByGenre(int genreId, Func<string, bool>? cityFilter = null, Func<long, bool>? notUserFilter = null)
    {
        var filter = (Entities.Offer offer) =>
        {
            bool isMatch = true;

            if (cityFilter != null)
            {
                isMatch = isMatch && cityFilter(offer.City);
            }

            if (notUserFilter != null)
            {
                isMatch = isMatch && notUserFilter(offer.Owner.Id);
            }

            return isMatch;
        };
        var culcOrderScoring = (Entities.Offer offer) =>
        {
            if (offer.Book.CountRating == 0)
            {
                return 0;
            }

            return (double)offer.Book.SumRating / offer.Book.CountRating;
        };
        using (DbCtx db = new DbCtx())
        {
            var offers = db.Offers
                .Include(offer => offer.Book)
                .Include(offer => offer.Book.Authors)
                .Include(offer => offer.Book.Genre)
                .Include(offer => offer.Owner)
                .Where(offer => offer.Book.Genre.Id == genreId)
                .AsEnumerable()
                .Where(filter)
                .OrderByDescending(culcOrderScoring)
                .Take(Constants.MaxOffersInSelection)
                .ToList()
                .ConvertAll(_responseMapper.MapOffer);
            return offers;
        }
    }

    public IEnumerable<Offer> GetOffersByTitleStart(string title)
    {
        using (DbCtx db = new DbCtx())
        {
            var offers = db.Offers
                .Include(offer => offer.Book)
                .Include(offer => offer.Book.Authors)
                .Include(offer => offer.Book.Genre)
                .Include(offer => offer.Owner)
                .AsEnumerable()
                .Where(offer => offer.Book.Title.ToLower().StartsWith(title.ToLower()))
                .ToList()
                .ConvertAll(_responseMapper.MapOffer);
            return offers;
        }
    }

    public IEnumerable<Offer> GetOffersByAuthorStart(string author)
    {
        var searchLambda = (Entities.Offer offer) =>
        {
            foreach (var bookAuthor in offer.Book.Authors)
            {
                if (bookAuthor.Name.ToLower().StartsWith(author.ToLower()))
                {
                    return true;
                }
            }

            return false;
        };
        using (DbCtx db = new DbCtx())
        {
            var offers = db.Offers
                .Include(offer => offer.Book)
                .Include(offer => offer.Book.Authors)
                .Include(offer => offer.Book.Genre)
                .Include(offer => offer.Owner)
                .AsEnumerable()
                .Where(offer => searchLambda(offer))
                .ToList()
                .ConvertAll(_responseMapper.MapOffer);

            return offers;
        }
    }

    public void AddOfferToFavorite(Guid offerId, long userId)
    {
        using (DbCtx db = new DbCtx())
        {
            var user = db.Users.Include(user => user.Offers).FirstOrDefault(user => user.Id == userId);
            var favoriteOffer = db.Offers.FirstOrDefault(offer => offer.Id == offerId);
            if (favoriteOffer == null)
            {
                throw new OfferNotFoundException($"Оффера с id={offerId} не найдено");
            }

            try
            {
                user.OffersNavigation.Add(favoriteOffer);
                db.SaveChanges();
            } catch (DbUpdateException ex)
            {
                var innerException = ex.InnerException;
                if (innerException is PostgresException postgresException && postgresException.ConstraintName == Constants.OffersUsersPK)
                {
                    throw new OfferAlreadyFavoriteException($"Оффер с id={offerId} уже добавлен в избранное" );
                }

                throw;
            }
        }
    }

    public void RemoveOfferFromFavorite(Guid offerId, long userId)
    {
        using (DbCtx db = new DbCtx())
        {
            var user = db.Users.Include(user => user.OffersNavigation).FirstOrDefault(user => user.Id == userId);

            var favoriteOffer = user.OffersNavigation.FirstOrDefault(offer => offer.Id == offerId);
            if (favoriteOffer == null)
            {
                throw new OfferAlreadyNotInFavoritesException($"Оффер с id={offerId} не в избранном пользователя");
            }

            user.OffersNavigation.Remove(favoriteOffer);
            db.SaveChanges();
        }
    }

    public void RemoveOffer(Guid offerId)
    {
        // var authUser = (AuthUser)ControllerContext.HttpContext.Items["User"];
        using (DbCtx db = new DbCtx())
        {
            var offer = db.Offers
                .Include(offer => offer.Owner)
                .FirstOrDefault(offer => offer.Id == offerId);
            if (offer == null)
            {
                throw new OfferNotFoundException($"Не найдено объявления с id={offerId}");
            }
            db.Offers.Remove(offer);
            db.SaveChanges();
        }
    }

    public bool IsUserOwnerOffer(Guid offerId, long userId)
    {
        using (DbCtx db = new DbCtx())
        {
            var offer = db.Offers
                .Include(offer => offer.Owner)
                .FirstOrDefault(offer => offer.Id == offerId);
            if (offer == null)
            {
                throw new OfferNotFoundException($"Не найдено объявления с id={offerId}");
            }

            return offer.Owner.Id == userId;
        }
    }

    public IEnumerable<Offer> GetFavoriteOffers(long userId)
    {
        using (DbCtx db = new DbCtx())
        {
            var user = db.Users
                .Include(user => user.OffersNavigation)
                .ThenInclude(offer => offer.Book)
                .ThenInclude(book => book.Authors)
                .Include(user => user.OffersNavigation)
                .ThenInclude(offer => offer.Book)
                .ThenInclude(book => book.Genre)
                .Include(user => user.OffersNavigation)
                .ThenInclude(offer => offer.Owner)
                .FirstOrDefault(user => user.Id == userId);

            return user.OffersNavigation.ToList().ConvertAll(_responseMapper.MapOffer);
        }
    }
}
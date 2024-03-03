using System.Net;
using BooksExchanger.Context;
using BooksExchanger.Controllers.Specs;
using BooksExchanger.Controllers.Specs.OffersCollector;
using BooksExchanger.Entities;
using BooksExchanger.Models;
using BooksExchanger.Repositories.Exeptions;
using BooksExchanger.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using ModerationStatus = BooksExchanger.Entities.ModerationStatus;
using Offer = BooksExchanger.Entities.Offer;

namespace BooksExchanger.Repositories.Implementations;

public class OffersCollectorRepository : IOffersCollectorRepository
{
    private ResponseMapper _responseMapper;

    public OffersCollectorRepository()
    {
        _responseMapper = new();
    }
    
    public Guid AddOffer(long userId, string title, string authors, string city, string? description, string? imageLink, decimal? price)
    {
        using (DbCtx db = new DbCtx())
        {
            var offer = new OffersCollector
            {
                Title = title,
                Authors = authors,
                City = city,
                Description = description,
                OwnerId = userId,
                Picture = imageLink,
                Price = price
            };
            db.OffersCollectors.Add(offer);
            db.SaveChanges();
            return offer.Id;
        }
    }

    public Guid SetRejectAndReason(Guid offerId, string reason)
    {
        using (DbCtx db = new DbCtx())
        {
            var offer = db.OffersCollectors.FirstOrDefault(offer => offer.Id == offerId);
            if (offer == null)
            {
                throw new OfferNotFoundException($"Не найдено оффера с id={offerId}");
            }

            offer.ModerationStatus = ModerationStatus.Rejected;
            offer.RejectReason = reason;
            db.SaveChanges();
            return offer.Id;
        }
    }

    public Guid MoveOfferToApprovedOffers(Guid offerId, long linkedBookId)
    {
        using (DbCtx db = new DbCtx())
        {
            var offer = db.OffersCollectors.FirstOrDefault(offer => offer.Id == offerId);
            if (offer == null)
            {
                throw new OfferNotFoundException($"Не найдено оффера с id={offerId}");
            }
            
            if (offer.ModerationStatus == ModerationStatus.Approved)
            {
                throw new OfferAlreadyApprovedException("Данный оффер уже подтвержден");
            }
            
            var linkedBook = db.Books.FirstOrDefault(book => book.Id == linkedBookId);
            if (linkedBook == null)
            {
                throw new BookNotFoundException($"Книги с id={linkedBookId} не найдено");
            }
            
            offer.ModerationStatus = ModerationStatus.Approved;
            var newOffer = new Offer
            {
                BookId = linkedBookId,
                OwnerId = offer.OwnerId,
                Description = offer.Description,
                Price = offer.Price,
                City = offer.City,
                Picture = offer.Picture
            };
            db.Offers.Add(newOffer);

            db.SaveChanges();
            return newOffer.Id;
        }
    }

    public bool CheckUserOwner(Guid offerId, long userId)
    {
        using (DbCtx db = new DbCtx())
        {
            var offer = db.OffersCollectors
                .Include(offer => offer.Owner)
                .FirstOrDefault(offer => offer.Id == offerId);

            if (offer == null)
            {
                throw new OfferNotFoundException($"Не найдено объявления с id={offerId}");
            }

            return offer.Owner.Id == userId;
        }
    }

    public void RemoveOffer(Guid offerId)
    {
        using (DbCtx db = new DbCtx())
        {
            var offer = db.OffersCollectors
                .Include(offer => offer.Owner)
                .FirstOrDefault(offer => offer.Id == offerId);

            if (offer == null)
            {
                throw new Repositories.Exeptions.OfferNotFoundException($"Не найдено объявления с id={offerId}");
            }
            db.OffersCollectors.Remove(offer);
            db.SaveChanges();
        }
    }

    public OfferCollector GetOffer(Guid? offerId)
    {
        using (DbCtx db = new DbCtx())
        {
            var offer = db.OffersCollectors
                .Include(offer => offer.Owner)
                .FirstOrDefault(offer => offer.Id == offerId);

            if (offer == null)
            {
                throw new OfferNotFoundException($"Не найдено объявления с id={offerId}");
            }

            return _responseMapper.MapOfferCollector(offer);
        }
    }
    
    public IEnumerable<OfferCollector> GetOffers(HashSet<Models.ModerationStatus> moderationStatusSet, long? userId = null)
    {
        var dbModerationStatusSet = new HashSet<ModerationStatus>();
        foreach (var moderationStatus in moderationStatusSet)
        {
            dbModerationStatusSet.Add(_responseMapper.MapModerationStatusToDb(moderationStatus));
        }

        using (DbCtx db = new DbCtx())
        {
            var offers = db.OffersCollectors
                .Include(offer => offer.Owner)
                .AsEnumerable()
                .Where(offer => dbModerationStatusSet.Contains(offer.ModerationStatus))
                .Where(offer => userId == null || offer.OwnerId == userId.Value );

            return offers.ToList().ConvertAll(_responseMapper.MapOfferCollector);
        }
    }
}
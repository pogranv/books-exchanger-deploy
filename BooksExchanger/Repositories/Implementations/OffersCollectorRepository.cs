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

/// <summary>
/// Репозиторий для работы с объявлениями о книгах.
/// </summary>
public class OffersCollectorRepository : IOffersCollectorRepository
{
    private ResponseMapper _responseMapper;
    
    /// <summary>
    /// Конструктор репозитория.
    /// </summary>
    public OffersCollectorRepository()
    {
        _responseMapper = new();
    }
    
    /// <summary>
    /// Добавляет новое объявление в базу данных.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя, создавшего объявление.</param>
    /// <param name="title">Название книги.</param>
    /// <param name="authors">Авторы книги.</param>
    /// <param name="city">Город, в котором расположена книга.</param>
    /// <param name="description">Описание книги (необязательно).</param>
    /// <param name="imageLink">Ссылка на изображение книги (необязательно).</param>
    /// <param name="price">Цена книги (необязательно).</param>
    /// <returns>Идентификатор созданного объявления.</returns>
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

    /// <summary>
    /// Устанавливает статус объявления как отклоненный и указывает причину отклонения.
    /// </summary>
    /// <param name="offerId">Идентификатор объявления.</param>
    /// <param name="reason">Причина отклонения объявления.</param>
    /// <returns>Идентификатор объявления.</returns>
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

    /// <summary>
    /// Перемещает объявление в подтвержденные, связывая его с указанной книгой.
    /// </summary>
    /// <param name="offerId">Идентификатор объявления.</param>
    /// <param name="linkedBookId">Идентификатор связанной книги.</param>
    /// <returns>Идентификатор нового объявления в подтвержденных.</returns>
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

    
    /// <summary>
    /// Проверяет, принадлежит ли объявление указанному пользователю.
    /// </summary>
    /// <param name="offerId">Идентификатор объявления.</param>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <returns>True, если объявление принадлежит пользователю и False, если нет.</returns>
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

    /// <summary>
    /// Удаляет объявление из базы данных.
    /// </summary>
    /// <param name="offerId">Идентификатор объявления.</param>
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

    /// <summary>
    /// Получает объявление по его идентификатору.
    /// </summary>
    /// <param name="offerId">Идентификатор объявления.</param>
    /// <returns>Объект объявления.</returns>
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
    
    /// <summary>
    /// Получает список объявлений согласно заданным статусам модерации и, опционально, идентификатору пользователя.
    /// </summary>
    /// <param name="moderationStatusSet">Набор статусов модерации.</param>
    /// <param name="userId">Идентификатор пользователя (необязательно).</param>
    /// <returns>Список объявлений.</returns>
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
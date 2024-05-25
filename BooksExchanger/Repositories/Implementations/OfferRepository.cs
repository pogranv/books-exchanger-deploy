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

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="OfferRepository"/>.
    /// </summary>
    public OfferRepository()
    {
        _responseMapper = new();
    }

    /// <summary>
    /// Получает предложение по его уникальному идентификатору.
    /// </summary>
    /// <param name="offerId">Уникальный идентификатор предложения.</param>
    /// <returns>Найденное предложение.</returns>
    /// <exception cref="OfferNotFoundException">Исключение выбрасывается, если предложение с указанным ID не найдено.</exception>
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
    
    /// <summary>
    /// Определяет, добавлено ли предложение в избранные у пользователя.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="offerId">Идентификатор предложения.</param>
    /// <returns>Возвращает true, если предложение находится в избранных у пользователя, иначе false.</returns>
    public bool IsFavoriteForUser(long userId, Guid offerId)
    {
        using (DbCtx db = new DbCtx())
        {

            var user = db.Users.Include(user => user.OffersNavigation).FirstOrDefault(user => user.Id == userId);
            return user.OffersNavigation.FirstOrDefault(offer => offer.Id == offerId) != null;
        }
    }
    
    /// <summary>
    /// Получает перечень предложений с возможностью фильтрации.
    /// </summary>
    /// <param name="genreFilter">Необязательный фильтр по идентификатору жанра.</param>
    /// <param name="cityFilter">Необязательный фильтр по городу.</param>
    /// <param name="userFilter">Необязательный фильтр по идентификатору пользователя.</param>
    /// <param name="notUserFilter">Необязательный фильтр для исключения предложений от определенного пользователя.</param>
    /// <returns>Перечень предложений, соответствующих заданным критериям.</returns>
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
    
    /// <summary>
    /// Получает предложения по жанру с возможностью дополнительной фильтрации.
    /// </summary>
    /// <param name="genreId">Идентификатор жанра.</param>
    /// <param name="cityFilter">Необязательный фильтр по городу.</param>
    /// <param name="notUserFilter">Необязательный фильтр для исключения предложений от определенного пользователя.</param>
    /// <returns>Перечень предложений по указанному жанру.</returns>
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

    /// <summary>
    /// Получает предложения, начинающиеся с заданного названия.
    /// </summary>
    /// <param name="title">Начало названия для поиска.</param>
    /// <returns>Перечень предложений с названиями, начинающимися с указанного текста.</returns>
    public IEnumerable<Offer> GetOffersByTitleStart(string title)
    {
        // Избегаем загрузки объектов в памяти, поэтому отказываемся от AsEnumerable и производим
        // фильтрацию на уровне базы данных
        using (DbCtx db = new DbCtx())
        {
            var offers = db.Offers
                .Include(offer => offer.Book)
                .ThenInclude(book => book.Authors)
                .Include(offer => offer.Book.Genre)
                .Include(offer => offer.Owner)
                .Where(offer => EF.Functions.Like(offer.Book.Title, title + "%"))
                .ToList() // Исполнение запроса к базе данных
                .ConvertAll(_responseMapper.MapOffer);

            return offers;
        }
    }

    /// <summary>
    /// Получает предложения по началу имени автора.
    /// </summary>
    /// <param name="author">Начало имени автора для поиска.</param>
    /// <returns>Перечень предложений, авторы которых начинаются с указанного текста.</returns>
    public IEnumerable<Offer> GetOffersByAuthorStart(string author)
    {
        // Избегаем загрузки объектов в памяти, поэтому отказываемся от AsEnumerable и производим
        // фильтрацию на уровне базы данных
        using (DbCtx db = new DbCtx())
        {
            var offers = db.Offers
                .Include(offer => offer.Book)
                .ThenInclude(book => book.Authors)
                .Include(offer => offer.Book.Genre)
                .Include(offer => offer.Owner)
                .Where(offer => offer.Book.Authors
                    .Any(bookAuthor => EF.Functions.Like(bookAuthor.Name, author + "%"))
                )   
                .ToList() // Исполнение запроса к базе данных
                .ConvertAll(_responseMapper.MapOffer);

            return offers;
        }
    }

    /// <summary>
    /// Добавляет предложение в избранные у пользователя.
    /// </summary>
    /// <param name="offerId">Идентификатор предложения.</param>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <exception cref="OfferNotFoundException">Исключение выбрасывается, если предложение с указанным ID не найдено.</exception>
    /// <exception cref="OfferAlreadyFavoriteException">Исключение выбрасывается, если предложение уже добавлено в избранные.</exception>
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

    /// <summary>
    /// Удаляет предложение из избранных у пользователя.
    /// </summary>
    /// <param name="offerId">Идентификатор предложения.</param>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <exception cref="OfferAlreadyNotInFavoritesException">Исключение выбрасывается, если предложения нет в избранных у пользователя.</exception>
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

    /// <summary>
    /// Удаляет предложение из репозитория.
    /// </summary>
    /// <param name="offerId">Идентификатор предложения, которое необходимо удалить.</param>
    /// <exception cref="OfferNotFoundException">Исключение выбрасывается, если предложение с указанным ID не найдено.</exception>
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

    /// <summary>
    /// Проверяет, является ли пользователь владельцем предложения.
    /// </summary>
    /// <param name="offerId">Идентификатор предложения.</param>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <returns>Возвращает true, если пользователь является владельцем предложения, иначе false.</returns>
    /// <exception cref="OfferNotFoundException">Исключение выбрасывается, если предложение с указанным ID не найдено.</exception>
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

    /// <summary>
    /// Получает список избранных предложений пользователя.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <returns>Перечень избранных предложений пользователя.</returns>
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
using BooksExchanger.Context;
using BooksExchanger.Controllers.Specs;
using BooksExchanger.Controllers.Specs.Offers;
using BooksExchanger.Entities;
using BooksExchanger.Models;
using BooksExchanger.Repositories.Exeptions;
using BooksExchanger.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Book = BooksExchanger.Entities.Book;
using Feedback = BooksExchanger.Entities.Feedback;

namespace BooksExchanger.Repositories.Implementations;


/// <summary>
/// Хранилище отзывов.
/// </summary>
public class FeedbackRepository : IFeedbackRepository
{
    private ResponseMapper _responseMapper;

    public FeedbackRepository()
    {
        _responseMapper = new();
    }

    /// <summary>
    /// Добавляет или обновляет отзыв.
    /// </summary>
    /// <param name="authorId"></param>
    /// <param name="offerId"></param>
    /// <param name="text"></param>
    /// <param name="estimation"></param>
    /// <param name="bookEstimation"></param>
    /// <returns></returns>
    /// <exception cref="OfferNotFoundException"></exception>
    public long UpdateFeedback(long authorId, Guid offerId, string text, int? estimation, BookEstimation bookEstimation)
    {
        using (DbCtx db = new DbCtx())
        {
            var offer = db.Offers
                .Include(offer => offer.Book)
                .Include(offer => offer.Book.Feedbacks)
                .FirstOrDefault(offer => offer.Id == offerId);
            if (offer == null)
            {
                throw new OfferNotFoundException($"Оффера с id={offerId} не найдено");
            }
            
            var userFeedback = offer.Book.Feedbacks.FirstOrDefault(feedback => feedback.GivenByUserId == authorId);
            offer.Book.CountRating = bookEstimation.CountRating;
            offer.Book.SumRating = bookEstimation.SumRating;
            if (userFeedback != null)
            {
                userFeedback.Feedback1 = text;
                userFeedback.Estimation = estimation;
                db.SaveChanges();
                return userFeedback.Id;
            }
        
            var newFeedback = new Feedback
            {
                Feedback1 = text,
                GivenByUserId = authorId,
                Estimation = estimation
            };
            offer.Book.Feedbacks.Add(newFeedback);
            db.SaveChanges();
            return newFeedback.Id;
        }
    }
    
    /// <summary>
    /// Добавляет новый отзыв.
    /// </summary>
    /// <param name="authorId">Автор отзыва.</param>
    /// <param name="offerId">id оффера.</param>
    /// <param name="text">Текст отзыва.</param>
    /// <param name="estimation">Оценка книги.</param>
    /// <returns></returns>
    public long InsertFeedback(long authorId, Guid offerId, string text, int? estimation)
    {
        using (DbCtx db = new DbCtx())
        {
            var offer = db.Offers
                .Include(offer => offer.Book)
                .Include(offer => offer.Book.Feedbacks)
                .FirstOrDefault(offer => offer.Id == offerId);
            if (offer == null)
            {
                throw new OfferNotFoundException($"Оффера с id={offerId} не найдено");
            }

            var newFeedback = new Feedback
            {
                Feedback1 = text,
                GivenByUserId = authorId,
                Estimation = estimation
            };
            offer.Book.Feedbacks.Add(newFeedback);
            if (estimation != null)
            {
                offer.Book.CountRating += 1;
                offer.Book.SumRating += estimation.Value;
            }
            db.SaveChanges();
            return newFeedback.Id;
        }
    }

    public Models.Feedback? GetFeedback(Guid offerId, long authorId)
    {
        using (DbCtx db = new DbCtx())
        {
            var offer = db.Offers
                .Include(offer => offer.Book)
                .Include(offer => offer.Book.Feedbacks)
                .ThenInclude(feed => feed.GivenByUser)
                .FirstOrDefault(offer => offer.Id == offerId);
            if (offer == null)
            {
                throw new OfferNotFoundException($"Оффера с id={offerId} не найдено");
            }

            var userFeedback = offer.Book.Feedbacks.FirstOrDefault(feedback => feedback.GivenByUserId == authorId);
            return userFeedback == null ? null :_responseMapper.MapFeedback(userFeedback, BuildBookEstimation(userFeedback.Book));
        }
    }

    public Models.Feedback? GetFeedback(long feedbackId)
    {
        using (DbCtx db = new DbCtx())
        {
            var feedback = db.Feedbacks.Include(feedback => feedback.GivenByUser).Include(feedback => feedback.Book).FirstOrDefault(feedback => feedback.Id == feedbackId);
            if (feedback == null)
            {
                return null;
            }
            return _responseMapper.MapFeedback(feedback, BuildBookEstimation(feedback.Book));
        }
    }

    public bool RemoveFeedback(long feedbackId)
    {
        using (DbCtx db = new DbCtx())
        {
            var feedback = db.Feedbacks.Include(feedback => feedback.Book).FirstOrDefault(feedback => feedback.Id == feedbackId);
            if (feedback == null)
            {
                return false;
            }

            if (feedback.Estimation != null)
            {
                feedback.Book.CountRating -= 1;
                feedback.Book.SumRating -= feedback.Estimation;
            }

            db.Feedbacks.Remove(feedback);
            db.SaveChanges();
            return true;
        }
    }

    public IEnumerable<Models.Feedback> GetFeedbacks(Guid offerId)
    {
        using (DbCtx db = new DbCtx())
        {
            var offer = db.Offers.Include(offer => offer.Book)
                .Include(offer => offer.Book.Feedbacks)
                .ThenInclude(feed => feed.GivenByUser)
                .FirstOrDefault(offer => offer.Id == offerId);
            if (offer == null)
            {
                throw new OfferNotFoundException($"Оффер с id={offerId} не найден");
            }
            
            var dbFeedbacks = offer.Book.Feedbacks.ToList();
            var feedbacks = dbFeedbacks.ConvertAll(feedback => _responseMapper.MapFeedback(feedback, BuildBookEstimation(feedback.Book)));
            return feedbacks;
        }
    }

    private BookEstimation BuildBookEstimation(Book book)
    {
        return new BookEstimation
        {
            CountRating = book.CountRating.Value,
            SumRating = book.SumRating.Value
        };
    }
}
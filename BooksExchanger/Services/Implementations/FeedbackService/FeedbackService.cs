using BooksExchanger.Controllers.Specs.Offers;
using BooksExchanger.Models;
using BooksExchanger.Repositories.Interfaces;
using BooksExchanger.Services.Exceptions;
using BooksExchanger.Services.Interfaces;
using BooksExchanger.Repositories.Exeptions;

namespace BooksExchanger.Services.Implementations.FeedbackService;

/// <summary>
/// Сервис отзывов.
/// </summary>
public class FeedbackService : IFeedbackService
{
    public IFeedbackRepository _feedbackRepository;

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="feedbackRepository">Хранилище отзывов.</param>
    public FeedbackService(IFeedbackRepository feedbackRepository)
    {
        _feedbackRepository = feedbackRepository;
    }

    /// <summary>
    /// Вычисляет оценку книги.
    /// </summary>
    /// <param name="currentEstimaion">Текущая оценка книги.</param>
    /// <param name="lastUserEstimation">Предыдущая оценка пользователя.</param>
    /// <param name="newUserEstimation">Новая оценка пользователя.</param>
    /// <returns>Обновленная оценка книги.</returns>
    private BookEstimation CulcBookEstimation(BookEstimation currentEstimaion, int? lastUserEstimation, int? newUserEstimation)
    {
        var result = new BookEstimation{CountRating = currentEstimaion.CountRating, SumRating = currentEstimaion.SumRating};
        if (lastUserEstimation.HasValue && newUserEstimation.HasValue)
        {
            result.SumRating = currentEstimaion.SumRating - lastUserEstimation.Value + newUserEstimation.Value;
        } 
        else if (!lastUserEstimation.HasValue && !newUserEstimation.HasValue)
        {
            return result;
        } 
        else if (lastUserEstimation.HasValue)
        {
            result.SumRating = currentEstimaion.SumRating - lastUserEstimation.Value;
        }
        else
        {
            result.SumRating = currentEstimaion.SumRating + newUserEstimation.Value;
            result.CountRating += 1;
        }

        return result;
    }

    /// <summary>
    /// Добавляет или обновляет отзыв.
    /// </summary>
    /// <param name="offerId">Идентификатор предложения.</param>
    /// <param name="authorId">Идентификатор автора отзыва.</param>
    /// <param name="text">Текст отзыва.</param>
    /// <param name="estimation">Оценка отзыва.</param>
    /// <returns>Идентификатор добавленного или обновленного отзыва.</returns>
    public long AddOrEditFeedback(Guid offerId, long authorId, string text="", int? estimation = null)
    {
        try
        {
            var feedback = _feedbackRepository.GetFeedback(offerId, authorId);
            if (feedback == null)
            {
                var id = _feedbackRepository.InsertFeedback(authorId, offerId, text, estimation);
                feedback = _feedbackRepository.GetFeedback(id);
            }
            var bookEstimation = CulcBookEstimation(feedback.BookEstimation, feedback.Estimation, estimation);
            var feedbackId = _feedbackRepository.UpdateFeedback(authorId, offerId, text, estimation, bookEstimation);
            return feedbackId;
        }
        catch (Repositories.Exeptions.OfferNotFoundException ex)
        {
            throw new Exceptions.OfferNotFoundException(ex.Message);
        }
        
    }

    /// <summary>
    /// Возвращает список отзывов для предложения.
    /// </summary>
    /// <param name="offerId">Идентификатор предложения.</param>
    /// <returns>Коллекция объектов отзывов.</returns>
    public IEnumerable<Feedback> GetFeedbacks(Guid offerId)
    {
        try
        {
            return _feedbackRepository.GetFeedbacks(offerId);
        }
        catch (Repositories.Exeptions.OfferNotFoundException ex)
        {
            throw new Exceptions.OfferNotFoundException(ex.Message);
        }
    }

    /// <summary>
    /// Удаляет отзыв.
    /// </summary>
    /// <param name="feedbackId">Идентификатор отзыва.</param>
    /// <param name="permissionsCheker">Функция проверки прав доступа.</param>
    public void RemoveFeedback(long feedbackId, Func<Models.Feedback, bool> permissionsCheker)
    {
        var feedback = _feedbackRepository.GetFeedback(feedbackId);
        if (feedback == null)
        {
            throw new FeedbackNotFoundException($"Фидбек с id={feedbackId} не найден");
        }
        if (!permissionsCheker(feedback))
        {
            throw new PermissionDenidedException();
        }

        _feedbackRepository.RemoveFeedback(feedbackId);
    }

    /// <summary>
    /// Фильтрует отзыв пользователя из списка отзывов.
    /// </summary>
    /// <param name="feedbacks">Список отзывов.</param>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <returns>Отзыв пользователя или null, если не найден.</returns>
    public Feedback? FilterOutUserFeedback(List<Feedback> feedbacks, long userId)
    {
        var userFeedback = feedbacks.FirstOrDefault(feedback => feedback.GivenByUserId == userId);
        if (userFeedback != null)
        {
            feedbacks.Remove(userFeedback);
        }
        return userFeedback;
    }
}
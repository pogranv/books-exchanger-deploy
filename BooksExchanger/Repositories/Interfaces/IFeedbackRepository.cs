using BooksExchanger.Models;

namespace BooksExchanger.Repositories.Interfaces;

/// <summary>
/// Интерфейс хранилища отзывов.
/// </summary>
public interface IFeedbackRepository
{
    /// <summary>
    /// Обновляет отзыв.
    /// </summary>
    /// <param name="authorId">id автора отзыва.</param>
    /// <param name="offerId">id оффера.</param>
    /// <param name="text">Текст отзыва.</param>
    /// <param name="estimation">Оценка от пользователя.</param>
    /// <param name="bookEstimation"></param>
    /// <returns>Оценка книги.</returns>
    long UpdateFeedback(long authorId, Guid offerId, string text, int? estimation, BookEstimation bookEstimation);

    /// <summary>
    /// Вставляет новый отзыв.
    /// </summary>
    /// <param name="authorId">Идентификатор автора отзыва.</param>
    /// <param name="offerId">Идентификатор предложения.</param>
    /// <param name="text">Текст отзыва.</param>
    /// <param name="estimation">Оценка отзыва (может быть null).</param>
    /// <returns>Идентификатор вновь вставленного отзыва.</returns>
    public long InsertFeedback(long authorId, Guid offerId, string text, int? estimation);

    /// <summary>
    /// Получает отзыв по идентификатору предложения и автора.
    /// </summary>
    /// <param name="offerId">Идентификатор предложения.</param>
    /// <param name="authorId">Идентификатор автора отзыва.</param>
    /// <returns>Объект отзыва или null, если отзыв не найден.</returns>
    Feedback? GetFeedback(Guid offerId, long authorId);
    
    /// <summary>
    /// Получает отзыв по идентификатору отзыва.
    /// </summary>
    /// <param name="feedbackId">Идентификатор отзыва.</param>
    /// <returns>Объект отзыва или null, если отзыв не найден.</returns>
    Feedback? GetFeedback(long feedbackId);

    /// <summary>
    /// Удаляет отзыв.
    /// </summary>
    /// <param name="feedbackId">Идентификатор отзыва.</param>
    /// <returns>True, если отзыв успешно удален; иначе false.</returns>
    bool RemoveFeedback(long feedbackId);

    /// <summary>
    /// Получает список отзывов по идентификатору предложения.
    /// </summary>
    /// <param name="offerId">Идентификатор предложения.</param>
    IEnumerable<Feedback> GetFeedbacks(Guid offerId);
}
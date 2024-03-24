namespace BooksExchanger.Services.Interfaces;

/// <summary>
/// Интерфейс сервиса для работы с отзывами.
/// </summary>
public interface IFeedbackService
{
    /// <summary>
    /// Добавляет или редактирует отзыв к предложению.
    /// </summary>
    /// <param name="offerId">Идентификатор предложения, для которого добавляется отзыв.</param>
    /// <param name="authorId">Идентификатор автора отзыва.</param>
    /// <param name="text">Текст отзыва (необязательно).</param>
    /// <param name="estimation">Оценка (необязательно).</param>
    /// <returns>Идентификатор созданного или обновленного отзыва.</returns>
    public long AddOrEditFeedback(Guid offerId, long authorId, string text = "", int? estimation = null);

    /// <summary>
    /// Получает отзывы для конкретного предложения.
    /// </summary>
    /// <param name="offerId">Идентификатор предложения, для которого запрашиваются отзывы.</param>
    /// <returns>Коллекцию отзывов для указанного предложения.</returns>
    public IEnumerable<Models.Feedback> GetFeedbacks(Guid offerId);

    /// <summary>
    /// Удаляет отзыв.
    /// </summary>
    /// <param name="feedbackId">Идентификатор отзыва для удаления.</param>
    /// <param name="permissionsCheker">Функция проверки разрешения на удаление отзыва для пользователя.</param>
    public void RemoveFeedback(long feedbackId, Func<Models.Feedback, bool> permissionsCheker);

    /// <summary>
    /// Отфильтровывает отзывы пользователя из списка всех отзывов.
    /// </summary>
    /// <param name="feedbacks">Список отзывов, из которого необходимо отфильтровать.</param>
    /// <param name="userId">Идентификатор пользователя, отзывы которого необходимо отфильтровать.</param>
    /// <returns>Отзыв пользователя, если он есть в списке.</returns>
    public Models.Feedback FilterOutUserFeedback(List<Models.Feedback> feedbacks, long userId);
}
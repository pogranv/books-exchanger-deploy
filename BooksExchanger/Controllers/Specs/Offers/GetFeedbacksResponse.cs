using BooksExchanger.Controllers.Specs.Common;

namespace BooksExchanger.Controllers.Specs.Offers;

/// <summary>
/// Модель ответа получения отзывов.
/// </summary>
public class GetFeedbacksResponse
{
    /// <summary>
    /// Отзыв пользователя (если есть).
    /// </summary>
    public Feedback? UserFeedback { get; set; }
    
    /// <summary>
    /// Отзыв других пользователей.
    /// </summary>
    public List<Feedback> Feedbacks { get; set; }
}
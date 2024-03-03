using System.ComponentModel.DataAnnotations;

namespace BooksExchanger.Controllers.Specs.Offers;

/// <summary>
/// Модель запроса на добавление отзыва.
/// </summary>
public class AddFeedbackRequest
{
    /// <summary>
    /// Оценка книги от 1 до 5.
    /// </summary>
    [Range(1, 5, ErrorMessage = "Оценка должна находиться в пределах [1; 5]")]
    public int? Estimation { get; set; }
    
    /// <summary>
    /// Текстовый отзыв пользователя на книгу.
    /// </summary>
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Максимальная длина 100 символов, минимальная - 3")]
    public string? Feedback { get; set; }
}
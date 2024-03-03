using System.ComponentModel.DataAnnotations;

namespace BooksExchanger.Controllers.Specs.User;

/// <summary>
/// Модель запроса на добавление администратора.
/// </summary>
public class SetNewAdminRequest
{
    /// <summary>
    /// email пользователя.
    /// </summary>
    [Required(ErrorMessage = "Не указана почта")]
    [StringLength(100, ErrorMessage = "Максимальная длина 100 символов")]
    [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный адрес электронной почты")]
    public string Email { get; set; }
}
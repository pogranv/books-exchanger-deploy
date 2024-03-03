using System.ComponentModel.DataAnnotations;

namespace BooksExchanger.Controllers.Specs.User;

/// <summary>
/// Модель запроса на верификацию пользователя.
/// </summary>
public class VerifyUserRequest
{
    /// <summary>
    /// Почта пользователя.
    /// </summary>
    [Required(ErrorMessage = "Не указана почта")]
    [StringLength(100, ErrorMessage = "Максимальная длина 100 символов")]
    [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный адрес электронной почты")]
    public string Email { get; set; }
    
    /// <summary>
    /// Код подтверждения.
    /// </summary>
    [Required(ErrorMessage = "Не указан код")]
    [StringLength(4, MinimumLength = 4, ErrorMessage = "Длина кода должна составлять 4 символа")]
    public string Code { get; set; }
}
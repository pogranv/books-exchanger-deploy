using System.ComponentModel.DataAnnotations;

namespace BooksExchanger.Models.Requests;

/// <summary>
/// Модель запроса на авторизацию пользователя.
/// </summary>
public class AuthRequest
{
    /// <summary>
    /// email пользователя.
    /// </summary>
    [Required(ErrorMessage = "Не указана почта")]
    [StringLength(100, ErrorMessage = "Максимальная длина 100 символов")]
    [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный адрес электронной почты")]
    public string Email { get; set; }
    
    /// <summary>
    /// Пароль пользователя.
    /// </summary>
    [Required(ErrorMessage = "Не указан пароль")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Максимальная длина - 50 символов, минимальная - 3")]
    public string Password { get; set; }
}
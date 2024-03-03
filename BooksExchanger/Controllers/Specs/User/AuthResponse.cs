namespace BooksExchanger.Models.Requests;

/// <summary>
/// Модель ответа на авторизацию пользователя.
/// </summary>
public class AuthResponse
{
    /// <summary>
    /// Токен авторизации.
    /// </summary>
    public string Token { get; set; }
}
namespace BooksExchanger.Services.Interfaces;

/// <summary>
/// Интерфейс сервиса для работы с пользовательскими данными.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Получает токен аутентификации для пользователя.
    /// </summary>
    /// <param name="email">Электронная почта пользователя.</param>
    /// <param name="password">Пароль пользователя.</param>
    /// <returns>Строка токена аутентификации.</returns>
    public string GetAuthToken(string email, string password);

    /// <summary>
    /// Регистрирует нового пользователя и возвращает токен аутентификации.
    /// </summary>
    /// <param name="name">Имя пользователя.</param>
    /// <param name="email">Электронная почта пользователя.</param>
    /// <param name="password">Пароль пользователя.</param>
    /// <returns>Строка токена аутентификации для регистрации.</returns>
    public string RegisterUser(string name, string email, string password);

    /// <summary>
    /// Проверяет, зарегистрирован ли пользователь с указанной электронной почтой.
    /// </summary>
    /// <param name="email">Электронная почта для проверки.</param>
    /// <returns>True, если пользователь с такой электронной почтой уже зарегистрирован; иначе false.</returns>
    public bool IsUserRegistered(string email);
    
    /// <summary>
    /// Проверяет, существует ли пользователь с указанным идентификатором.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <returns>True, если пользователь существует; иначе false.</returns>
    public bool IsUserExist(long userId);

    /// <summary>
    /// Назначает роль администратора пользователю по его электронной почте.
    /// </summary>
    /// <param name="email">Электронная почта пользователя.</param>
    public void SetAdmin(string email);
}
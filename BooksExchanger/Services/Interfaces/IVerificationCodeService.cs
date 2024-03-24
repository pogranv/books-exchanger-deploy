using BooksExchanger.Models;

namespace BooksExchanger.Services.Interfaces;

/// <summary>
/// Определяет интерфейс сервиса для работы с кодами верификации пользователей.
/// </summary>
public interface IVerificationCodeService
{
    /// <summary>
    /// Проверяет пользователя, используя адрес электронной почты и код верификации.
    /// </summary>
    /// <param name="userEmail">Адрес электронной почты пользователя.</param>
    /// <param name="verificationCode">Код верификации.</param>
    /// <returns>Краткую информацию о пользователе, если верификация прошла успешно.</returns>
    public ShortUserInfo VerifyUser(string userEmail, string verificationCode);
        
    /// <summary>
    /// Отправляет код верификации пользователю и запоминает информацию о нем.
    /// </summary>
    /// <param name="userInfo">Краткая информация о пользователе.</param>
    public void SendCodeAndRememberUser(ShortUserInfo userInfo);
}
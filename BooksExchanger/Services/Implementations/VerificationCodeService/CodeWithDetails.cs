using BooksExchanger.Models;

namespace BooksExchanger.VerificationCodesManager;

/// <summary>
/// Содержит информацию о коде подтверждения с деталями пользователя.
/// </summary>
internal class CodeWithDetails
{
    /// <summary>
    /// Пользователь, связанный с кодом подтверждения.
    /// </summary>
    public ShortUserInfo User { get; }
    
    /// <summary>
    /// Код подтверждения.
    /// </summary>
    public string VerificationCode { get; }

    /// <summary>
    /// Время создания кода подтверждения.
    /// </summary>
    private readonly DateTime _timeOfCreation;
    
    /// <summary>
    /// Конструктор класса CodeWithDetails.
    /// </summary>
    /// <param name="user">Информация о пользователе.</param>
    /// <param name="verificationCode">Код подтверждения.</param>
    public CodeWithDetails(ShortUserInfo user, string verificationCode) 
    {
        User = user;
        VerificationCode = verificationCode;
        _timeOfCreation = DateTime.Now;
    }

    /// <summary>
    /// Проверяет соответствие указанного кода сохраненному коду подтверждения.
    /// </summary>
    /// <param name="code">Код для проверки.</param>
    /// <returns>true, если указанный код совпадает с кодом подтверждения; иначе false.</returns>
    public bool IsEqualsVerificationCode(string code)
    {
        return VerificationCode.Equals(code);
    }

    /// <summary>
    /// Проверяет, просрочено ли время жизни кода подтверждения.
    /// </summary>
    /// <param name="lifeTimeMinutes">Время жизни кода подтверждения в минутах.</param>
    /// <returns>true, если время жизни кода подтверждения истекло; иначе false.</returns>
    public bool IsDurationOfExistsOverdue(int lifeTimeMinutes)
    {
        var now = DateTime.Now;
        return lifeTimeMinutes < now.Subtract(_timeOfCreation).Minutes;
    }
}


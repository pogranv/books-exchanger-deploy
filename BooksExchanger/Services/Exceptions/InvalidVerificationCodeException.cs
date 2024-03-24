namespace BooksExchanger.VerificationCodesManager.Exceptions;

/// <summary>
/// Неверный код подтверждения.
/// </summary>
public class InvalidVerificationCodeException : Exception
{
    /// <summary>
    /// Конструктор.
    /// </summary>
    public InvalidVerificationCodeException()
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public InvalidVerificationCodeException(string message) : base(message)
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public InvalidVerificationCodeException(string message, Exception inner) : base(message, inner)
    {
    }
}
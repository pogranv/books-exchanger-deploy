namespace BooksExchanger.VerificationCodesManager;

/// <summary>
/// Параметры письма.
/// </summary>
/// <param name="EmailSenderHeader">Заголовок отпарвителя.</param>
/// <param name="EmailHeader">Заголовок письма.</param>
/// <param name="MessageBody">Текст письма.</param>
public sealed record EmailParams(string EmailSenderHeader, string EmailHeader, string MessageBody);



namespace BooksExchanger.VerificationCodesManager;

/// <summary>
/// Настройки для отправки письма.
/// </summary>
public class SmtpGmailSettings
{
    /// <summary>
    /// Строка для отправки письма.
    /// </summary>
    public string SmtpGmailString { get; private set; }
    
    /// <summary>
    /// Порт для отправки письма.
    /// </summary>
    public int SmtpGmailPort { get; private set; }
    
    /// <summary>
    /// Ключ для отправки письма.
    /// </summary>
    public string GmailKey { get; private set; }
    
    /// <summary>
    /// Адрес электронной почты.
    /// </summary>
    public string EmailForSmtp { get; private set; }

    /// <summary>
    /// Билдер настроек.
    /// </summary>
    /// <returns></returns>
    public static SmtpGmailSettings BuildDefault()
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("SmtpConnecionSettings");
        int.TryParse(config["SmtpGmailPort"], out int port);
        return new SmtpGmailSettings
        {
            SmtpGmailString = config["SmtpGmailString"],
            SmtpGmailPort = port,
            GmailKey = config["GmailKey"],
            EmailForSmtp = config["EmailForSmtp"],
        };
    }
}


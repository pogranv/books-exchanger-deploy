using System.Net;
using System.Net.Mail;
using System.Collections.Concurrent;
using BooksExchanger.Models;
using BooksExchanger.Services.Implementations.UserService.Exceptions;
using BooksExchanger.Services.Interfaces;
using Obshajka.VerificationCodesManager.Exceptions;

namespace Obshajka.VerificationCodesManager
{
    public class VerificationCodeService : IVerificationCodeService
    {

        private ConcurrentDictionary<string, CodeWithDetails> _emailToDetails;
        private static readonly Random s_getVerificationCode = new();

        private readonly Timer _periodic;
        private readonly int _codesLifeTimeMinutes;

        private readonly string _emailSenderHeader;
        private readonly string _emailHeader;
        private readonly string _messageBody;

        private readonly ILogger<VerificationCodeService> _logger;

        private static readonly SmtpGmailSettings s_smtpGmailSettings;

        static VerificationCodeService()
        {
            s_smtpGmailSettings = SmtpGmailSettings.BuildDefault();
        }

        public VerificationCodeService(int codesLifeDurationMinutes, EmailParams emailParams)
        {
            _logger = LoggerFactory.Create(options => options.AddConsole()).CreateLogger<VerificationCodeService>();

            _emailToDetails = new();

            _codesLifeTimeMinutes = codesLifeDurationMinutes;
            TimerCallback tm = new TimerCallback(UpdateVerificationCodes);
            _periodic = new Timer(tm, null, 0, _codesLifeTimeMinutes * 60 * 1000); // minutes * seconds * milisec

            _emailSenderHeader = emailParams.EmailSenderHeader;
            _emailHeader = emailParams.EmailHeader;
            _messageBody = emailParams.MessageBody;
        }

        public void SendCodeAndRememberUser(ShortUserInfo userInfo)
        {
            if (_emailToDetails.ContainsKey(userInfo.Email))
            {
                throw new UserAlreadyWaitConfirmationException($"С момента последнего запроса кода подтверждения должно пройти { _codesLifeTimeMinutes } минут.");
            }

            string verificationCode = s_getVerificationCode.Next(1000, 10000).ToString();
            

            using (MailMessage mailMessage = new MailMessage("akkforfox5@gmail.com", "aisharipov@edu.hse.ru")) // TODO: move to settings
            {
                mailMessage.Subject = _emailHeader;
                mailMessage.Body = _messageBody + verificationCode;
                mailMessage.IsBodyHtml = false;
                using (SmtpClient smtpClient = new SmtpClient(s_smtpGmailSettings.SmtpGmailString, s_smtpGmailSettings.SmtpGmailPort))
                {
                    smtpClient.EnableSsl = true;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(s_smtpGmailSettings.EmailForSmtp, s_smtpGmailSettings.GmailKey);
                    smtpClient.Send(mailMessage);
                }
            }
            _emailToDetails[userInfo.Email] = new CodeWithDetails(userInfo, verificationCode);
        }

        
        public ShortUserInfo VerifyUser(string userEmail, string verificationCode)
        {
            if (string.IsNullOrEmpty(userEmail) || !_emailToDetails.ContainsKey(userEmail))
            {
                throw new UserNotFoundException($"С момента последней отправки кода подтверждения прошло больше {_codesLifeTimeMinutes} минут. Повторите запрос на отправку кода подтверждения.");
            }

            var codeWithDetails = _emailToDetails[userEmail]; //.IsEqualsVerificationCode(verificationCode);
            if (!codeWithDetails.IsEqualsVerificationCode(verificationCode))
            {
                throw new InvalidVerificationCodeException("Неверный код");
            }

            return codeWithDetails.User;
        }

        private void UpdateVerificationCodes(object obj)
        {
            var toRemoveEmails = _emailToDetails.Where(item => item.Value.IsDurationOfExistsOverdue(_codesLifeTimeMinutes));
            foreach (var item in toRemoveEmails)
            {
                if (_emailToDetails.ContainsKey(item.Key))
                {
                    string userEmail = item.Key;
                    if (_emailToDetails.TryRemove(item))
                    {
                        _logger.LogInformation($"{TimeOnly.FromDateTime(DateTime.Now)}: Пользователь {userEmail} не подтвердил свою почту и был удален из очереди ожидания");
                    }
                }
            }
        }
    }
}

using BooksExchanger.Models;

namespace BooksExchanger.Services.Interfaces;

public interface IVerificationCodeService
{
    public ShortUserInfo VerifyUser(string userEmail, string verificationCode);
        
    public void SendCodeAndRememberUser(ShortUserInfo userInfo);
}
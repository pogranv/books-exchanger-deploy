

using BooksExchanger.Models;

namespace Obshajka.VerificationCodesManager
{

    internal class CodeWithDetails
    {
        public ShortUserInfo User { get; }
        public string VerificationCode { get; }

        private readonly DateTime _timeOfCreation;
        public CodeWithDetails(ShortUserInfo user, string verificationCode) 
        {
            User = user;
            VerificationCode = verificationCode;
            _timeOfCreation = DateTime.Now;
        }

        public bool IsEqualsVerificationCode(string code)
        {
            return VerificationCode.Equals(code);
        }

        public bool IsDurationOfExistsOverdue(int lifeTimeMinutes)
        {
            var now = DateTime.Now;
            return lifeTimeMinutes < now.Subtract(_timeOfCreation).Minutes;
        }
    }
}

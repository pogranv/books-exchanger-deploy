namespace BooksExchanger.VerificationCodesManager.Exceptions
{

    /// <summary>
    /// Пользователь уже ожидает подтверждения.
    /// </summary>
    [Serializable]
    public class UserAlreadyWaitConfirmationException : Exception
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public UserAlreadyWaitConfirmationException() { }
        
        /// <summary>
        /// Конструктор.
        /// </summary>
        public UserAlreadyWaitConfirmationException(string message) : base(message) { }
        
        /// <summary>
        /// Конструктор.
        /// </summary>
        public UserAlreadyWaitConfirmationException(string message, Exception inner) : base(message, inner) { }
        protected UserAlreadyWaitConfirmationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}

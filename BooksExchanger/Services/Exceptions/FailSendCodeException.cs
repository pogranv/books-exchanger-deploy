namespace BooksExchanger.VerificationCodesManager.Exceptions
{

    /// <summary>
    /// Не удалось отправить код подтверждения.
    /// </summary>
    [Serializable]
    public class FailSendCodeException : Exception
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public FailSendCodeException() { }
        
        /// <summary>
        /// Конструктор.
        /// </summary>
        public FailSendCodeException(string message) : base(message) { }
        
        /// <summary>
        /// Конструктор.
        /// </summary>
        public FailSendCodeException(string message, Exception inner) : base(message, inner) { }
        
        /// <summary>
        /// Конструктор.
        /// </summary>
        protected FailSendCodeException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}

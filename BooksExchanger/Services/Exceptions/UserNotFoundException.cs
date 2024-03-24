namespace BooksExchanger.Services.Implementations.UserService.Exceptions
{

    /// <summary>
    /// Пользователя не существует.
    /// </summary>
    [Serializable]
    public class UserNotFoundException : Exception
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public UserNotFoundException() { }
        
        /// <summary>
        /// Конструктор.
        /// </summary>
        public UserNotFoundException(string message) : base(message) { }
        
        /// <summary>
        /// Конструктор.
        /// </summary>
        public UserNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected UserNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}

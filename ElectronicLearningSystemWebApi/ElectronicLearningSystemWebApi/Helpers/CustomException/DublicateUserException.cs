namespace ElectronicLearningSystemWebApi.Helpers.CustomException
{
    /// <summary>
    /// Пользователь уже существует в системе.
    /// </summary>
    public class DublicateUserException : ApplicationException
    {
        /// <summary>
        /// ctor.
        /// </summary>
        public DublicateUserException() { }

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="message">Ошибка.</param>
        public DublicateUserException(string message) 
            : base(message) 
        {
        
        }

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="message">Ошибка.</param>
        /// <param name="inner">Стек.</param>
        public DublicateUserException(string message, Exception inner) 
            : base(message, inner) 
        { 
        
        }
    }
}

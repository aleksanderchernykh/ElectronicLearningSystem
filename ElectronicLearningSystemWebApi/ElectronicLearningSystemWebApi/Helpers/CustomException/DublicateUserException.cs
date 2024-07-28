using System;
using System.Runtime.Serialization;

namespace ElectronicLearningSystemWebApi.Helpers.CustomException
{
    /// <summary>
    /// Пользователь уже существует в системе.
    /// </summary>
    public class DublicateUserException : ApplicationException
    {
        public DublicateUserException() { }

        public DublicateUserException(string message) : base(message) { }

        public DublicateUserException(string message, Exception inner) : base(message, inner) { }
    }
}

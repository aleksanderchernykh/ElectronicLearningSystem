namespace ElectronicLearningSystem.Core.CustomException
{
    public class DublicateUserException : ApplicationException
    {
        public DublicateUserException() { }

        public DublicateUserException(string message)
            : base(message)
        {

        }

        public DublicateUserException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}

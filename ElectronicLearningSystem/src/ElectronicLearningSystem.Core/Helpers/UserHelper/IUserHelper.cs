namespace ElectronicLearningSystem.Core.Helpers
{
    public interface IUserHelper
    {
        /// <summary>
        /// Получение идентификатора текущего пользователя.
        /// </summary>
        Guid GetCurrentUserId();
    }
}
namespace ElectronicLearningSystemWebApi.Enums
{
    /// <summary>
    /// Перечисление событий для логгера.
    /// </summary>
    public enum EventLoggerEnum
    {
        /// <summary>
        /// Ошибка на уровне БД.
        /// </summary>
        DataBaseException = 1000,

        /// <summary>
        /// Найден дубликат пользователя.
        /// </summary>
        DublicateUserException = 1001,
        
        /// <summary>
        /// Ошибка при отправке сообщения;
        /// </summary>
        EmailSendingException = 1002,

        /// <summary>
        /// Ошибка маппинга сущностей.
        /// </summary>
        InvalidMapEntity = 1003
    }
}

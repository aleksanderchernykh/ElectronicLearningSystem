namespace ElectronicLearningSystem.Core.Extensions
{
    /// <summary>
    /// Методы расширения для Guid.
    /// </summary>
    public static class GuidExtension
    {
        /// <summary>
        /// Проверка дефолтного значения Guid.
        /// </summary>
        public static void ThrowIsDefault(this Guid value)
        {
            if (value == default)
                throw new ArgumentNullException(nameof(value));
        }
    }
}

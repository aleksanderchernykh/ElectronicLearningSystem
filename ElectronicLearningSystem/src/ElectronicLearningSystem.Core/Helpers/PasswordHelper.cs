namespace ElectronicLearningSystem.Core.Helpers
{
    /// <summary>
    /// Хелпер работы с паролями.
    /// </summary>
    public static class PasswordHelper
    {
        /// <summary>
        /// Хеширование пароля.
        /// </summary>
        /// <param name="password"></param>
        /// <returns>Захешированный пароль.</returns>
        public static string HashPassword(string password)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(password, nameof(password));

            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        /// <summary>
        /// Проверка захешированного пароля.
        /// </summary>
        /// <param name="storedHash">Захерированный пароль.</param>
        /// <param name="password">Переданный пароль.</param>
        /// <returns>Логическое значение равенства паролей.</returns>
        public static bool VerifyPassword(string storedHash, string password)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(storedHash, nameof(storedHash));
            ArgumentException.ThrowIfNullOrWhiteSpace(password, nameof(password));

            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }
    }
}

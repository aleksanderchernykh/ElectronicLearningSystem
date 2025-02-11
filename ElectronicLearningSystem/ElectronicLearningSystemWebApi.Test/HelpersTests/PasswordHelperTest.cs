using ElectronicLearningSystemWebApi.Helpers;
using Xunit;

namespace ElectronicLearningSystemWebApi.Test.HelpersTests
{
    public class PasswordHelperTest
    {
        [Fact]
        public void HashPassword_ValidPassword_ReturnsHashedPassword()
        {
            // Arrange
            string password = "SecurePassword123!";

            // Act
            string hashedPassword = PasswordHelper.HashPassword(password);

            // Assert
            Assert.NotNull(hashedPassword);
            Assert.NotEmpty(hashedPassword);
            Assert.True(hashedPassword.Length > 0);
        }

        [Fact]
        public void HashPassword_EmptyPassword_ThrowsArgumentException()
        {
            // Arrange
            string password = "";

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => PasswordHelper.HashPassword(password));
            Assert.Equal("password", exception.ParamName);
        }

        [Fact]
        public void VerifyPassword_CorrectPassword_ReturnsTrue()
        {
            // Arrange
            string password = "SecurePassword123!";
            string hashedPassword = PasswordHelper.HashPassword(password);

            // Act
            bool result = PasswordHelper.VerifyPassword(hashedPassword, password);

            // Assert

            Assert.True(result);
        }

        [Fact]
        public void VerifyPassword_IncorrectPassword_ReturnsFalse()
        {
            // Arrange
            string password = "SecurePassword123!";
            string wrongPassword = "WrongPassword123!";
            string hashedPassword = PasswordHelper.HashPassword(password);

            // Act
            bool result = PasswordHelper.VerifyPassword(hashedPassword, wrongPassword);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void VerifyPassword_EmptyStoredHash_ThrowsArgumentException()
        {
            // Arrange
            string storedHash = "";
            string password = "SecurePassword123!";

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => PasswordHelper.VerifyPassword(storedHash, password));
            Assert.Equal("storedHash", exception.ParamName);
        }

        [Fact]
        public void VerifyPassword_EmptyPassword_ThrowsArgumentException()
        {
            // Arrange
            string storedHash = PasswordHelper.HashPassword("SecurePassword123!");
            string password = "";

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => PasswordHelper.VerifyPassword(storedHash, password));
            Assert.Equal("password", exception.ParamName);
        }
    }
}

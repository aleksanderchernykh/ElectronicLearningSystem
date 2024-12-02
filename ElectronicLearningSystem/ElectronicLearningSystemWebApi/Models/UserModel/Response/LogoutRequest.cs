namespace ElectronicLearningSystemWebApi.Models.UserModel.Response
{
    public class LogoutRequest
    {
        /// <summary>
        /// Логин пользователя.
        /// </summary>
        public required string Login { get; set; }
    }
}

namespace ElectronicLearningSystem.Application.Models.UserModel.Response
{
    public class AccessTokenResponse
    {
        /// <summary>
        /// Токен доступа.
        /// </summary>
        public required string AccessToken { get; set; }

        /// <summary>
        /// Токен для обновления.
        /// </summary>
        public required string RefreshToken { get; set; }
    }
}

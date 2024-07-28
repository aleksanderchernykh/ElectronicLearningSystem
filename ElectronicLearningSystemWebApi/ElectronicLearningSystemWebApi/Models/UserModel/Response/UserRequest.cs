namespace ElectronicLearningSystemWebApi.Models.UserModel.Response
{
    public class UserRequest : UserLoginRequest
    {
        public required string Email { get; set; }
    }
}

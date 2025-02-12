namespace ElectronicLearningSystemWebApi.Models.UserModel.Response
{
    public class UserResponse : BaseResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public Guid RoleId { get; set; }
        public bool IsLocked { get; set; }
    }
}

using ElectronicLearningSystemWebApi.Models.UserModel;

namespace ElectronicLearningSystemWebApi.Models
{
    public abstract class BaseDTO
    {
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Guid? CreatedById { get; set; }
        public Guid? ModifiedById { get; set; }
    }
}

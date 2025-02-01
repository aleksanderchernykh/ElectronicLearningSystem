using ElectronicLearningSystemWebApi.Models.UserModel;

namespace ElectronicLearningSystemWebApi.Models
{
    public abstract class EntityBase
    {
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }

        public Guid? CreatedById { get; set; }
        public UserEntity? CreatedBy { get; set; }

        public Guid? ModifiedById { get; set; }
        public UserEntity? ModifiedBy { get; set; }
    }
}

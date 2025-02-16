namespace ElectronicLearningSystem.Application.Models
{
    public abstract class BaseResponse
    {
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Guid? CreatedById { get; set; }
        public Guid? ModifiedById { get; set; }
    }
}

namespace ElectronicLearningSystemWebApi.Models
{
    public abstract class EntityBase
    {
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }

        public DateTime ModifiedBy { get; set; }
    }
}

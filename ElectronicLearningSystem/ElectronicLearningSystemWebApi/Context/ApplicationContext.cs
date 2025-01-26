using ElectronicLearningSystemWebApi.Models.CommentModel;
using ElectronicLearningSystemWebApi.Models.GroupModel;
using ElectronicLearningSystemWebApi.Models.NotificationModel;
using ElectronicLearningSystemWebApi.Models.NotificationTypeModel;
using ElectronicLearningSystemWebApi.Models.RoleModel;
using ElectronicLearningSystemWebApi.Models.StudentModel;
using ElectronicLearningSystemWebApi.Models.TaskModel;
using ElectronicLearningSystemWebApi.Models.UserModel;
using Microsoft.EntityFrameworkCore;

namespace ElectronicLearningSystemWebApi.Context
{
    public class ApplicationContext(DbContextOptions<ApplicationContext> options) 
        : DbContext(options)
    {
        public DbSet<UserEntity> User => Set<UserEntity>();
        public DbSet<RoleEntity> Role => Set<RoleEntity>();
        public DbSet<GroupEntity> Group => Set<GroupEntity>();
        public DbSet<StudentProfileEntity> StudentProfile => Set<StudentProfileEntity>();
        public DbSet<NotificationTypeEntity> NotificationType => Set<NotificationTypeEntity>();
        public DbSet<NotificationEntity> Notification => Set<NotificationEntity>();
        public DbSet<TaskEntity> Task => Set<TaskEntity>();
        public DbSet<CommentEntity> Comment => Set<CommentEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NotificationTypeEntity>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<UserEntity>(entity =>
            {
                entity
                    .HasOne(u => u.CreatedBy)
                    .WithMany()
                    .HasForeignKey(u => u.CreatedById)
                    .OnDelete(DeleteBehavior.NoAction);

                entity
                    .HasOne(u => u.ModifiedBy)
                    .WithMany()
                    .HasForeignKey(u => u.ModifiedById)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<RoleEntity>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<UserEntity>()
                .HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey(u => u.RoleId);

            modelBuilder.Entity<GroupEntity>()
                .HasOne(u => u.Tutor)
                .WithMany()
                .HasForeignKey(u => u.TutorId);

            modelBuilder.Entity<StudentProfileEntity>()
                .HasOne(u => u.User)
                .WithOne()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<NotificationEntity>()
                .HasOne(u => u.NotificationType)
                .WithMany()
                .HasForeignKey(u => u.NotificationTypeId);

            modelBuilder.Entity<NotificationEntity>()
                .HasOne(u => u.Recipient)
                .WithMany()
                .HasForeignKey(u => u.RecipientId);

            modelBuilder.Entity<TaskEntity>()
                .HasOne(t => t.Owner)
                .WithMany()
                .HasForeignKey(t => t.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TaskEntity>()
                .HasOne(t => t.Student)
                .WithMany()
                .HasForeignKey(t => t.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CommentEntity>()
                .HasOne(t => t.Task)
                .WithMany()
                .HasForeignKey(t => t.TaskId);

            base.OnModelCreating(modelBuilder);
        }
    }
}

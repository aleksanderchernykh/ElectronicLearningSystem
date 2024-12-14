using ElectronicLearningSystemWebApi.Models.GroupModel;
using ElectronicLearningSystemWebApi.Models.StudentModel;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);

            modelBuilder.Entity<GroupEntity>()
                .HasOne(u => u.Tutor)
                .WithMany(r => r.Groups)
                .HasForeignKey(u => u.TutorId);

            modelBuilder.Entity<StudentProfileEntity>()
                .HasOne(u => u.User)
                .WithOne(r => r.StudentProfile)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}

using ElectronicLearningSystemWebApi.Models.GroupModel;
using ElectronicLearningSystemWebApi.Models.StudentModel;
using ElectronicLearningSystemWebApi.Models.UserModel;
using Microsoft.EntityFrameworkCore;

namespace ElectronicLearningSystemWebApi.Context
{
    public class ApplicationContext(DbContextOptions<ApplicationContext> options) 
        : DbContext(options)
    {
        public DbSet<User> User => Set<User>();
        public DbSet<Role> Role => Set<Role>();
        public DbSet<Group> Group => Set<Group>();
        public DbSet<StudentProfile> StudentProfile => Set<StudentProfile>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);

            modelBuilder.Entity<Group>()
                .HasOne(u => u.Tutor)
                .WithMany(r => r.Groups)
                .HasForeignKey(u => u.TutorId);

            modelBuilder.Entity<StudentProfile>()
                .HasOne(u => u.User)
                .WithOne(r => r.StudentProfile)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}

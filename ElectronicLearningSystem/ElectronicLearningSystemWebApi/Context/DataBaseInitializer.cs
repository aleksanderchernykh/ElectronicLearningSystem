using ElectronicLearningSystemWebApi.Enums;
using ElectronicLearningSystemCore.Extensions;
using ElectronicLearningSystemWebApi.Models.UserModel;
using ElectronicLearningSystemWebApi.Helpers;
using ElectronicLearningSystemWebApi.Models.NotificationTypeModel;
using ElectronicLearningSystemWebApi.Models.RoleModel;

namespace ElectronicLearningSystemWebApi.Context
{
    public static class DataBaseInitializer
    {
        public static void Initialize(ApplicationContext context)
        {
            if (context.User.Any())
            {
                return;
            }

            var administrator = new RoleEntity
            {
                Id = (Guid)UserRoleEnum.Admin.GetAmbientValue(),
                Name = "Администратор"
            };

            var student = new RoleEntity
            {
                Id = (Guid)UserRoleEnum.Student.GetAmbientValue(),
                Name = "Студент"
            };

            var teacher = new RoleEntity
            {
                Id = (Guid)UserRoleEnum.Teacher.GetAmbientValue(),
                Name = "Преподаватель"
            };

            context.Role.AddRange(
                administrator,
                teacher,
                student);

            context.User.Add(new UserEntity
            { 
                Id = Guid.NewGuid(),
                Role = administrator,
                Login = "Administrator",
                Password = PasswordHelper.HashPassword("Administrator"),
                Email = "aleksandr.chernykh3@gmail.com",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now
            });

            context.User.Add(new UserEntity
            {
                Id = Guid.NewGuid(),
                Role = teacher,
                Login = "Teacher",
                Password = PasswordHelper.HashPassword("Teacher"),
                Email = "aleksandr.chernykh3@gmail.com",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now
            });

            context.User.Add(new UserEntity
            {
                Id = Guid.NewGuid(),
                Role = student,
                Login = "Student",
                Password = PasswordHelper.HashPassword("Student"),
                Email = "aleksandr.chernykh3@gmail.com",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now
            });

            context.NotificationType.Add(new NotificationTypeEntity 
            { 
                Id = (Guid)NotificationTypeEnum.Comment.GetAmbientValue(),
                Name = "Отправлен новый комментарий" 
            });

            context.NotificationType.Add(new NotificationTypeEntity
            {
                Id = (Guid)NotificationTypeEnum.Message.GetAmbientValue(),
                Name = "Отправлено новое сообщение"
            });

            context.SaveChanges();
        }
    }
}

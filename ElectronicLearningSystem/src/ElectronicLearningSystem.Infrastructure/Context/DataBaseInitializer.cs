using ElectronicLearningSystem.Core.Enums;
using ElectronicLearningSystem.Infrastructure.Models.NotificationTypeModel;
using ElectronicLearningSystem.Infrastructure.Models.RoleModel;
using ElectronicLearningSystem.Infrastructure.Models.UserModel;
using ElectronicLearningSystem.Core.Extensions;
using ElectronicLearningSystem.Core.Helpers;

namespace ElectronicLearningSystem.Infrastructure.Context
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
                Name = "Administrator"
            };

            var student = new RoleEntity
            {
                Id = (Guid)UserRoleEnum.Student.GetAmbientValue(),
                Name = "Teacher"
            };

            var teacher = new RoleEntity
            {
                Id = (Guid)UserRoleEnum.Teacher.GetAmbientValue(),
                Name = "Student"
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
                Password = PasswordHelper.HashPassword("RsQOluzt"),
                Email = "aleksandr.chernykh3@gmail.com",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now
            });

            context.User.Add(new UserEntity
            {
                Id = Guid.NewGuid(),
                Role = teacher,
                Login = "Teacher",
                Password = PasswordHelper.HashPassword("TZewZmJj"),
                Email = "aleksandr.chernykh3@gmail.com",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now
            });

            context.User.Add(new UserEntity
            {
                Id = Guid.NewGuid(),
                Role = student,
                Login = "Student",
                Password = PasswordHelper.HashPassword("FXRCYoix"),
                Email = "aleksandr.chernykh3@gmail.com",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now
            });

            context.NotificationType.Add(new NotificationTypeEntity
            {
                Id = (Guid)NotificationTypeEnum.Comment.GetAmbientValue(),
                Name = "New comment sent"
            });

            context.NotificationType.Add(new NotificationTypeEntity
            {
                Id = (Guid)NotificationTypeEnum.Message.GetAmbientValue(),
                Name = "A new message has been sent"
            });

            context.SaveChanges();
        }
    }
}

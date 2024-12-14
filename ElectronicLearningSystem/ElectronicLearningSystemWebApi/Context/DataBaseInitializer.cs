using ElectronicLearningSystemWebApi.Enums;
using ElectronicLearningSystemCore.Extensions;
using ElectronicLearningSystemWebApi.Models.UserModel;
using ElectronicLearningSystemWebApi.Helpers;

namespace ElectronicLearningSystemWebApi.Context
{
    public static class DataBaseInitializer
    {
        public static void Initialize(ApplicationContext context)
        {
            if (context.User.Any() ||
                context.Role.Any())
            {
                return;
            }

            var administrator = new RoleEntity
            {
                Id = (Guid)UserRoleEnum.Admin.GetAmbientValue(),
                Name = "Администратор"
            };

            context.Role.AddRange(
                administrator, 
                new RoleEntity {
                    Id = (Guid)UserRoleEnum.Teacher.GetAmbientValue(),
                    Name = "Преподаватель"
                },
                new RoleEntity
                {
                    Id = (Guid)UserRoleEnum.Student.GetAmbientValue(),
                    Name = "Студент"
                });

            context.User.Add(new UserEntity
                { 
                    Id = Guid.NewGuid(),
                    Role = administrator,
                    Login = "Supervisor",
                    Password = PasswordHelper.HashPassword("Supervisor"),
                    Email = "aleksandr.chernykh3@gmail.com"
                });

            context.SaveChanges();
        }
    }
}

using ElectronicLearningSystemWebApi.Enums;
using ElectronicLearningSystemCore.Extensions;
using ElectronicLearningSystemWebApi.Helpers;
using ElectronicLearningSystemWebApi.Models.UserModel;

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

            var administrator = new Role
            {
                Id = (Guid)UserRoleEnum.Admin.GetAmbientValue(),
                Name = "Администратор"
            };

            context.Role.AddRange(
                administrator, 
                new Role {
                    Id = (Guid)UserRoleEnum.Teacher.GetAmbientValue(),
                    Name = "Преподаватель"
                },
                new Role
                {
                    Id = (Guid)UserRoleEnum.Student.GetAmbientValue(),
                    Name = "Студент"
                });

            context.User.Add(new User
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

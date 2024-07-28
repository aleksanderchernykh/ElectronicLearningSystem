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
                Id = new Guid("02bc926f-9c56-4fb9-bc8e-68bbe2e87c17"),
                Name = "Администратор"
            };

            context.Role.AddRange(
                administrator, 
                new Role {
                    Id = new Guid("c0eb7e9a-b913-4cd0-bf70-146fc48764ba"),
                    Name = "Преподаватель"
                },
                new Role
                {
                    Id = new Guid("86b8ca0b-85ce-4aca-b911-28836645ebc7"),
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

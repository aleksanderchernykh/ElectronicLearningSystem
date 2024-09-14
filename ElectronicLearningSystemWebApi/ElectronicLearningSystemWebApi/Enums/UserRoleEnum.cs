using System.ComponentModel;

namespace ElectronicLearningSystemWebApi.Enums
{
    /// <summary>
    /// Перечисление идентификаторов ролей.
    /// </summary>
    public enum UserRoleEnum
    {   
        /// <summary>
        /// Администратор.
        /// </summary>
        [AmbientValue(typeof(Guid), "02bc926f-9c56-4fb9-bc8e-68bbe2e87c17")]
        Admin = 1,

        /// <summary>
        /// Преподаватель.
        /// </summary>
        [AmbientValue(typeof(Guid), "c0eb7e9a-b913-4cd0-bf70-146fc48764ba")]
        Teacher = 2,

        /// <summary>
        /// Студент.
        /// </summary>
        [AmbientValue(typeof(Guid), "86b8ca0b-85ce-4aca-b911-28836645ebc7")]
        Student = 3,
    }
}

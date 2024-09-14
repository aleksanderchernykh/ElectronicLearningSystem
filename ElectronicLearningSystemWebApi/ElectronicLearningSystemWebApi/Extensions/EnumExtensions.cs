using System.ComponentModel;
using System.Reflection;

namespace ElectronicLearningSystemWebApi.Extensions
{
    /// <summary>
    /// Методы расширения для Guid.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Получение идентифкатора из перечисления в пользовательском аттрибуте.
        /// </summary>
        /// <param name="enumVal">Значение перечисления</param>
        /// <returns>Идентификатор.</returns>
        public static object GetAmbientValue(this Enum enumVal)
        {
            Type type = enumVal.GetType();
            MemberInfo[] memInfo = type.GetMember(enumVal.ToString());
            object[] attributes = memInfo[0].GetCustomAttributes(typeof(AmbientValueAttribute), false);

            if (attributes == null || attributes.Length == 0)
            {
                return default;
            }

            return ((AmbientValueAttribute)attributes[0]).Value;
        }
    }
}

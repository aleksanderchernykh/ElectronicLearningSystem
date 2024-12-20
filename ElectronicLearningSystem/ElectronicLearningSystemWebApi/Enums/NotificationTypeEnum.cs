using System.ComponentModel;

namespace ElectronicLearningSystemWebApi.Enums
{
    public enum NotificationTypeEnum
    {
        /// <summary>
        /// Комментарий.
        /// </summary>
        [AmbientValue(typeof(Guid), "2cbbcb67-fb42-4dcc-ae89-61f93a283d10")]
        Comment = 1,

        /// <summary>
        /// Сообщение.
        /// </summary>
        [AmbientValue(typeof(Guid), "d569e3db-3daa-435c-9a02-2f21d19132f9")]
        Message = 2,
    }
}

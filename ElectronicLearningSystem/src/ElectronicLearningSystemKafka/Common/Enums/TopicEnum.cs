using System.ComponentModel;

namespace ElectronicLearningSystemKafka.Common.Enums
{
    public enum TopicEnum
    {
        [AmbientValue(typeof(string), "EmailSendingTopic")]
        EmailSending,

        [AmbientValue(typeof(string), "EmailSendingTopicRetry")]
        EmailSendingRetry
    }
}

using System.ComponentModel;

namespace ElectronicLearningSystem.Kafka.Common.Enums
{
    public enum TopicEnum
    {
        [AmbientValue(typeof(string), "EmailSendingTopic")]
        EmailSending,

        [AmbientValue(typeof(string), "EmailSendingTopicRetry")]
        EmailSendingRetry
    }
}

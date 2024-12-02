using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicLearningSystemKafka.Common.Enums
{
    public enum TopicEnum
    {
        [AmbientValue(typeof(string), "EmailSendingTopic")]
        EmailSending
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSendingService.Common
{
    public class EmailSettings
    {
        public string SmtpServerUrl { get; set; } = string.Empty;
        public int SmtpPort { get; set; }
        public string SenderEmailAddress { get; set; } = string.Empty;
        public string SenderPassword { get; set; } = string.Empty;
    }
}

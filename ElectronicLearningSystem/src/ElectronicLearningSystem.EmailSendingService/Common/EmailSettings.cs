namespace ElectronicLearningSystem.EmailSendingService.Common
{
    public class EmailSettings
    {
        public string SmtpServerUrl { get; set; } = string.Empty;
        public int SmtpPort { get; set; }
        public string SenderEmailAddress { get; set; } = string.Empty;
        public string SenderPassword { get; set; } = string.Empty;
    }
}

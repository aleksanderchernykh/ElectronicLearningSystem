using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Logging;
using ElectronicLearningSystemKafka.Common.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ElectronicLearningSystem.Core.Enums;
using ElectronicLearningSystem.EmailSendingService.Common;

namespace ElectronicLearningSystem.EmailSendingService
{
    public class EmailSender
    {
        protected readonly ILogger<EmailSender> _logger;
        protected readonly IConfiguration _configuration;

        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _emailFrom;
        private readonly string _emailPassword;
        private readonly EmailSettings _emailSettings;

        public EmailSender(ILogger<EmailSender> logger, IOptions<EmailSettings> emailSettings)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _emailSettings = emailSettings?.Value ?? throw new ArgumentNullException(nameof(emailSettings));

            if (string.IsNullOrEmpty(_emailSettings.SmtpServerUrl))
                throw new ArgumentNullException(nameof(_emailSettings.SmtpServerUrl), "SMTP server URL is not configured.");

            if (string.IsNullOrEmpty(_emailSettings.SenderEmailAddress))
                throw new ArgumentNullException(nameof(_emailSettings.SenderEmailAddress), "Sender email address is not configured.");

            if (string.IsNullOrEmpty(_emailSettings.SenderPassword))
                throw new ArgumentNullException(nameof(_emailSettings.SenderPassword), "Sender email password is not configured.");
        }

        public async Task SendEmailAsync(Email email)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(email);
                ArgumentNullException.ThrowIfNull(email.Recipients);
                ArgumentException.ThrowIfNullOrWhiteSpace(email.Subject);
                ArgumentException.ThrowIfNullOrWhiteSpace(email.Text);

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_emailFrom),
                    Subject = email.Subject,
                    Body = email.Text,
                    IsBodyHtml = true
                };

                foreach (var address in email.Recipients)
                {
                    mailMessage.To.Add(address);
                }

                var smtpClient = new SmtpClient(_smtpServer)
                {
                    Port = _smtpPort,
                    Credentials = new NetworkCredential(_emailFrom, _emailPassword),
                    EnableSsl = true,
                    UseDefaultCredentials = false
                };

                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError((int)EventLoggerEnum.EmailSendingException,
                    $"Error sending the message Subject: {email.Subject} Text: {email.Text}. Error: {ex}");
            }
        }
    }
}

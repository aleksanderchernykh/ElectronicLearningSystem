using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Logging;
using ElectronicLearningSystemWebApi.Enums;
using ElectronicLearningSystemKafka.Common.Models;
using Microsoft.Extensions.Configuration;

namespace EmailSendingService
{
    public class EmailSender
    {
        protected readonly ILogger<EmailSender> _logger;
        protected readonly IConfiguration _configuration;

        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _emailFrom;
        private readonly string _emailPassword;

        public EmailSender(ILogger<EmailSender> logger, IConfiguration configuration)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            _smtpServer = _configuration.GetValue<string>("Email:SmtpServerUrl")
                ?? throw new ArgumentNullException(nameof(_smtpServer), "SMTP server URL is not configured.");

            _emailFrom = _configuration.GetValue<string>("Email:SenderEmailAddress")
                ?? throw new ArgumentNullException(nameof(_emailFrom), "Sender email address is not configured.");

            _emailPassword = _configuration.GetValue<string>("Email:SenderPassword")
                ?? throw new ArgumentNullException(nameof(_emailPassword), "Sender email password is not configured.");

            _smtpPort = _configuration.GetValue<int>("Email:SmtpPort");
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

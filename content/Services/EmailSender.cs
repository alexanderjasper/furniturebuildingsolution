using System;
using FurnitureBuildingSolution.Entities;
using FurnitureBuildingSolution.Helpers;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace FurnitureBuildingSolution.Services
{
    public interface IEmailSender
    {
        bool Send(Email email);
    }

    public class EmailSender : IEmailSender
    {
        private readonly AppSettings _appSettings;
        private readonly ILogger<EmailSender> _logger;
        public EmailSender(IOptions<AppSettings> appSettings, ILogger<EmailSender> logger)
        {
            _appSettings = appSettings.Value;
            _logger = logger;
        }

        public bool Send(Email email)
        {
            var message = GetMessage(email);

            return SendViaSmtp(message);
        }

        private bool SendViaSmtp(MimeMessage message)
        {
            try
            {
                var credentials = _appSettings.EmailSettings;

                SmtpClient client = new SmtpClient
                {
                    ServerCertificateValidationCallback = (s, c, h, e) =>
                    {
                        return true;
                        // return c.GetCertHashString() == credentials.CertificateThumbprint;
                    }
                };
                client.Connect(credentials.SmtpAddress, credentials.SmtpPort, MailKit.Security.SecureSocketOptions.Auto);
                client.Authenticate(credentials.Username, credentials.Password);

                client.Send(message);
                client.Disconnect(true);
                client.Dispose();

                return true;
            }
            catch (Exception exception)
            {
                _logger.LogWarning(exception.ToString());
                return false;
            }
        }

        private MimeMessage GetMessage(Email email)
        {
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = email.HtmlBody;
            bodyBuilder.TextBody = email.TextBody;

            var message = new MimeMessage();

            var from = new MailboxAddress("Shelfer", "no-reply@shelfer.dk");
            message.From.Add(from);

            var to = new MailboxAddress(email.Receiver);
            message.To.Add(to);

            message.Subject = email.Subject;

            message.Body = bodyBuilder.ToMessageBody();

            return message;
        }
    }
}

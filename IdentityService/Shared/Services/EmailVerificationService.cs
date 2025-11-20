using IdentityService.Shared.Cofigurations;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Exam_System.Shared.Services
{
    public class EmailVerificationService
    {
        private readonly EmailSettings _settings;

        public EmailVerificationService(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }
        public string GenerateVerificationCode()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        public async Task SendVerificationEmailAsync(string toEmail, string code)
        {
            using (var client = new SmtpClient(_settings.SmtpHost, _settings.SmtpPort))
            {

                client.Credentials = new NetworkCredential(_settings.SmtpUser, _settings.SmtpPass);
                client.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_settings.FromEmail, "My App"),
                    Subject = "Email Verification Code",
                    Body = $"Your verification code is: {code}",
                    IsBodyHtml = false
                };

                mailMessage.To.Add(toEmail.Trim());

                await client.SendMailAsync(mailMessage);
            }
        }
    }
}

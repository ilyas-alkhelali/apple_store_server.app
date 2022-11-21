using Apple_Store_Db_Server.Dto;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace Apple_Store_Db_Server.Services
{    
    public interface IEmailService
    {
        void SendEmail(string To, string Subject, string Body);
    }
    public class EmailService: IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public void SendEmail(string To, string Subject, string Body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailSendSettings:FromEmail").Value));
            email.To.Add(MailboxAddress.Parse(To));
            email.Subject = Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = Body };

            using var smtp = new SmtpClient();
            smtp.Connect(_config.GetSection("EmailSendSettings:FromEmailHost").Value, 587,
                SecureSocketOptions.StartTls);
            smtp.Authenticate(_config.GetSection("EmailSendSettings:FromEmail").Value,
                _config.GetSection("EmailSendSettings:FromEmailPassword").Value);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}

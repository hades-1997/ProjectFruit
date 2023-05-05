using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace ProjectFruit.Helpers
{
    public class MailHelper
    {
        private IConfiguration configuration;

        public MailHelper(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public bool Send(string from, string to, string subject, string body)
        {
            try
            {
                var host = configuration["EmailSettings:SmtpServer"].ToString();
                var port = int.Parse(configuration["EmailSettings:SmtpPort"].ToString());
                var username = configuration["EmailSettings:UserName"].ToString();
                var password = configuration["EmailSettings:Password"].ToString();
                var enable = bool.Parse(configuration["EmailSettings:EnableSsl"].ToString());

                var smtpClient = new SmtpClient
                {
                    Host = host,
                    Port = port,
                    EnableSsl = enable,
                    Credentials = new NetworkCredential(username, password)
                };
                var mailMessage = new MailMessage(from, to, subject, body);

                mailMessage.IsBodyHtml = true;
                smtpClient.Send(mailMessage);
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}

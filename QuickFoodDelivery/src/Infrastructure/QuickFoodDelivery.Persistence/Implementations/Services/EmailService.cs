using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using QuickFoodDelivery.Application.Abstractions.Services;

namespace QuickFoodDelivery.Persistence.Implementations.Services
{
    public class EmailService:IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string ToEmail, string subject, string body, bool ishtml)
        {
            SmtpClient smtpClient = new SmtpClient(_configuration["Email:Host"], Convert.ToInt32(_configuration["Email:Port"]));
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential(_configuration["Email:LoginEmail"], _configuration["Email:Password"]);
            MailAddress from = new MailAddress(_configuration["Email:LoginEmail"]);
            MailAddress to = new MailAddress(ToEmail);
            MailMessage message = new MailMessage(from, to);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = ishtml;
            await smtpClient.SendMailAsync(message);
        }
    }
}

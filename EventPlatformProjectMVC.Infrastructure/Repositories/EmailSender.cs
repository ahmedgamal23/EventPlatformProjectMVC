using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EventPlatformProjectMVC.Infrastructure.Repositories
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var sysEmail = "am5679456@gmail.com";
            var sysPassword = "xnrd lglj raav qwbt";
            
            var client = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(sysEmail, sysPassword),
                EnableSsl = true
            };

            var mail = new MailMessage(sysEmail, email)
            {
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true,
                From = new MailAddress(sysEmail)
            };

            mail.To.Add(email);

            return client.SendMailAsync(mail);
        }
    }
}

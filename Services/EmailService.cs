using NOOKX_Project.Models;
using NOOKX_Project.Services.Interfaces;
using NOOKX_Project.ViewModels.MailSenderVM;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net.Mail;
using System.Net;

namespace NOOKX_Project.Services
{
    public class EmailService : IEmailService
    {
        private readonly MailSetting _emailSettings;
        private readonly SmtpClient _smtpClient;

        public EmailService(IOptions<MailSetting> emailSettings)
        {

            _emailSettings = emailSettings.Value;
            SmtpClient smtpClient = new SmtpClient()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential(_emailSettings.Email, _emailSettings.Password)
            };
            _smtpClient = smtpClient;
        }

        public void SendMessage(MailRequestVM mailRequest)
        {
            MailMessage newMessage = new MailMessage(_emailSettings.Email, mailRequest.ToEmail)
            {
                Subject = mailRequest.Subject,
                Body = mailRequest.Body,
                IsBodyHtml = true
            };
            _smtpClient.Send(newMessage);
        }
    }
}

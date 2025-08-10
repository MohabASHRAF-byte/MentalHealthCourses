using MentalHealthcare.Domain.Repositories.EmailSender;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Infrastructure.Repositories
{
    public class EmailSenderWithAttachment : IEmailSenderWithAttachment
    {
        public async Task SendEmailWithAttachmentAsync(string email, string subject, string htmlMessage, string attachmentPath)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Mental Healthcare", "your-email@example.com"));
            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = subject;

            var builder = new BodyBuilder
            {
                HtmlBody = htmlMessage
            };

            if (File.Exists(attachmentPath))
            {
                builder.Attachments.Add(attachmentPath);
            }

            message.Body = builder.ToMessageBody();

            using var client = new MailKit.Net.Smtp.SmtpClient(); 
            await client.ConnectAsync("smtp.gmail.com", 587, false);
            await client.AuthenticateAsync("committeeformentalhealth20@gmail.com", "fhli ftwe hcvv ibql");
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}

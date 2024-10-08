using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;

namespace MentalHealthcare.Application.Utitlites.EmailProvider;

public class EmailProvider(IConfiguration configuration): IEmailSender

{
    
    public async Task SendEmailAsync(string email, string subject, string sendMessage)
    {
        var fromMail = configuration["Mail:senderMail"] ;
        var fromMailPassword = configuration["Mail:senderPassword"];

        var message = new MailMessage()
        {
            From = new MailAddress(fromMail!),
            Subject = subject,
            Body = sendMessage,
            To = { email }
        };
        var smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential(fromMail, fromMailPassword),
            EnableSsl = true
        };
        await smtpClient.SendMailAsync(message);
    }
        
}
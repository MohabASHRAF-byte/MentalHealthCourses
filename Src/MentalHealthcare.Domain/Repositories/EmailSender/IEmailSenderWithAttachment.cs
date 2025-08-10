using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Domain.Repositories.EmailSender
{
    public interface IEmailSenderWithAttachment
    {

        Task SendEmailWithAttachmentAsync(string email, string subject, string htmlMessage, string attachmentPath);


    }
}

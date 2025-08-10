using MentalHealthcare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Domain.Repositories
{
    public interface ICertificateRepository
    {
        public Task<Certificate> GenerateCertificateAsync(string userName, string courseName, string userEmail, int adminId);
        Task SendCertificateEmailAsync(string userEmail, string pdfFilePath);}
}

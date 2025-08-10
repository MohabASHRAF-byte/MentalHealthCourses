using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Domain.Entities
{
    public class Certificate
    {
        public int CertificateId { get; set; }
        public string UserEmail { get; set; }
        public string UserName { get; set; }
        public string CourseName { get; set; }
        public string CertificatePath { get; set; }  // Path to the generated certificate PDF
        public DateTime DateGenerated { get; set; }
        public string? CertificateLink { get; set; }  // Google Drive link to the certificate

        public int AdminId { get; set; }
        public Admin Admin { get; set; }

    }
}

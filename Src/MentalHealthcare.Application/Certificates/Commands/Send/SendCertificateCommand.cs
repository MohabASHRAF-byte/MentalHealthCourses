using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Certificates.Commands.Send
{
    public class SendCertificateCommand : IRequest<Unit>
    {
        public string UserEmail { get; set; }
        public string CertificatePath { get; set; }
    }
}

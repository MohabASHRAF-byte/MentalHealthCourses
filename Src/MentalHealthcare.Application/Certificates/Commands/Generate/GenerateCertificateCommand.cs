using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Certificates.Commands.Generate
{
    public class GenerateCertificateCommand : IRequest<string>
    {

        public string userName { get; set; }

        public string courseName { get; set; }
        public string userEmail { get; set; }
    }
}

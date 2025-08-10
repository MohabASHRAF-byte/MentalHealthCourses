using MediatR;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Certificates.Commands.Send
{
    public class SendCertificateCommandHandler(ICertificateRepository certificateRepository,
            ILogger<SendCertificateCommandHandler> logger) : IRequestHandler<SendCertificateCommand , Unit>
    {
        public async Task<Unit> Handle(SendCertificateCommand request, CancellationToken cancellationToken)
        {


            try
            {
                await certificateRepository.SendCertificateEmailAsync(request.UserEmail, request.CertificatePath);
                return Unit.Value;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error sending certificate to {Email}", request.UserEmail);
                throw;
            }






        }
    }
}

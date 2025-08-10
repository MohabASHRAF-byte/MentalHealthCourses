using AutoMapper;
using MediatR;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Certificates.Commands.Generate
{
    public class GenerateCertificateCommandHandler(
          ICertificateRepository certificateRepository,
            ILogger<GenerateCertificateCommandHandler> logger,
            IUserContext userContext, IMapper mapper
) : IRequestHandler<GenerateCertificateCommand, string>
    {
        public async Task<string> Handle(GenerateCertificateCommand request, CancellationToken cancellationToken)
        {

            try
            {
                // Step 1: Authorize User (Ensure User is Admin)
                logger.LogInformation("Authorizing user to generate certificate.");
                var currentUser = userContext.EnsureAuthorizedUser([UserRoles.Admin], logger);
                logger.LogInformation("User {UserId} authorized to generate certificates.", currentUser.Id);

                // Step 2: Generate and Save the Certificate using the Repository
                logger.LogInformation("Generating certificate for user {UserEmail}.", request.userEmail);


                var newCertificate = mapper.Map<Domain.Entities.Certificate>(request);


                newCertificate.AdminId = (int)currentUser.AdminId;





                var certificate = await certificateRepository.GenerateCertificateAsync(
                   request.userName,
    request.courseName,
    request.userEmail,
    (int)currentUser.AdminId
                // Assuming currentUser contains AdminId or some reference for admin
                );

                // Step 3: Return the certificate path or appropriate response
                logger.LogInformation("Certificate generated and saved at {CertificatePath}.", certificate.CertificatePath);
                return certificate.CertificatePath;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while generating certificate for {UserEmail}.", request.userEmail);
                throw new ApplicationException("An error occurred while generating the certificate.", ex);
            }

        }
    }
}

using MediatR;
using MentalHealthcare.Application.Authors.Commands.Delete;
using MentalHealthcare.Application.BunnyServices;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Instructors.Command.Delete
{
    public class DeleteInstructorCommandHandler(
    ILogger<DeleteInstructorCommandHandler> logger,
    IInstructorRepository instructorRepository,
    IConfiguration configuration
    ) : IRequestHandler<DeleteInstructorCommand>
    { public async Task Handle(DeleteInstructorCommand request, CancellationToken cancellationToken)
        {logger.LogInformation("Delete Instructor");
            var Ins = await instructorRepository.GetInstructorById(request.InstructorId);
            if (Ins is null) { logger.LogWarning("Selected Instructor Not Existed !!"); }
            else
            {var bunny = new BunnyClient(configuration);
                var imgName = Ins.ImageUrl;
                await bunny.DeleteFile(imgName, Global.AdvertisementFolderName);
                await instructorRepository.DeleteInstructor(request.InstructorId);}
            
        }
    }
}

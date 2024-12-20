using MediatR;
using MentalHealthcare.Application.BunnyServices;
using MentalHealthcare.Application.Instructors.Command.Delete;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Instructors.Commands.Delete
{
    public class DeleteInstructorCommandHandler(
    ILogger<DeleteInstructorCommandHandler> logger,
    IInstructorRepository instructorRepository,
    IConfiguration configuration
    ) : IRequestHandler<DeleteInstructorCommand>
    {
        public async Task Handle(DeleteInstructorCommand request, CancellationToken cancellationToken)
        {


            //todo : Add Auth
            var ad = await instructorRepository.GetInstructorById(request.InstructorId);
            var bunny = new BunnyClient(configuration);
            
                var imgName = GetImageName(ad.ImageUrl);
                await bunny.DeleteFileAsync(imgName, Global.AdvertisementFolderName);
            
            await instructorRepository.DeleteInstructor(request.InstructorId);


        }




        private string GetImageName(string url)
        {
            return url.Split('/').Last();
        }





    }
}

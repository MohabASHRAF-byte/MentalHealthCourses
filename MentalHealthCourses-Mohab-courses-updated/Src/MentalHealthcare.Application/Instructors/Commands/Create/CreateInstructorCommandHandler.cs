using AutoMapper;
using MediatR;
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

namespace MentalHealthcare.Application.Instructors.Command.Create
{
    public class CreateInstructorCommandHandler(
    ILogger<CreateInstructorCommandHandler> logger,
    IInstructorRepository instructorRepository,
    IConfiguration configuration,
    IMapper mapper
) : IRequestHandler<CreateInstructorCommand, int>
    {
        public async Task<int> Handle(CreateInstructorCommand request, CancellationToken cancellationToken)
        {//todo 
            // add auth

            CheckPhotosSize(ref request);
            var NewInstructor = mapper.Map<Domain.Entities.Instructor>(request);


            var bunny = new BunnyClient(configuration);


            var newImageName = $"{NewInstructor.InstructorId}_{NewInstructor.ImageUrl}.jpeg";

            // Upload the image to BunnyCDN
            var response = await bunny.UploadFileAsync(request.ImageUrl, newImageName, Global.InstructorFolderName);

            if (!response.IsSuccessful || response.Url == null)
            {
                logger.LogWarning(@"Could not upload Instructor {ad} error msg :{mg}", request.Name,
                   response.Message ?? "No message provided"
               );
                throw new Exception("Image upload failed.");
            }

            await instructorRepository.AddInstructor(NewInstructor);

            return NewInstructor.InstructorId;

        }

        private void CheckPhotosSize(ref CreateInstructorCommand request)
        {
            if (request.ImageUrl != null)
            {
                var imgSizeInMb = request.ImageUrl.Length / (1 << 20);
                if (imgSizeInMb > Global.InstructorImgSize)
                {
                    logger.LogWarning($"try to upload img with size {imgSizeInMb} ");
                    throw new Exception($"Image size cannot be greater than {Global.InstructorImgSize} MB");
                }

            }
            else { throw new Exception("Image of Author is required."); }
        }
    }
}
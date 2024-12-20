using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Advertisement.Commands.Update;
using MentalHealthcare.Application.BunnyServices;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MentalHealthcare.Application.Instructors.Command.Update
{
    public class UpdateInstructorCommandHandler(
    ILogger<UpdateInstructorCommandHandler> logger,
    IInstructorRepository instructorRepository,
    IMapper mapper,
    IConfiguration configuration
) : IRequestHandler<UpdateInstructorCommand, int>
    {
        public async Task<int> Handle(UpdateInstructorCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation($"Starting update process for Instructor ID {request.InstructorId}.");

            if (request.InstructorId == null)
                return 0;

            ValidateImageSizes(request);

            var Instructor = await instructorRepository.GetInstructorById((int)request.InstructorId);
            UpdateAdvertisementDetails(ref Instructor, request);

            var bunnyClient = new BunnyClient(configuration);

            HandleExistingImages(ref Instructor, request, bunnyClient);
            UploadNewImage(ref Instructor, request, bunnyClient);

            await instructorRepository.UpdateInstructorAsync(Instructor);
            return Instructor.InstructorId;
        }


        private void ValidateImageSizes(UpdateInstructorCommand request)
        {
            var imageSizeInMb = request.ImageUrl.Length / (1 << 20);
            if (imageSizeInMb > Global.InstructorImgSize)
            {
                logger.LogWarning($"Attempted to upload an image exceeding the allowed size: {imageSizeInMb} MB.");
                throw new Exception($"Image size cannot exceed {Global.InstructorImgSize} MB.");
            }

        }
        private void UpdateAdvertisementDetails(ref Domain.Entities.Instructor instructor, UpdateInstructorCommand request)
        {


            if (!request.Name.IsNullOrEmpty())
                instructor.Name = request.Name!;

            if (!request.About.IsNullOrEmpty())
                instructor.About = request.About!;
        }


        private void HandleExistingImages(
      ref Domain.Entities.Instructor instructor,
      UpdateInstructorCommand request,
      BunnyClient bunnyClient)
        {
            if (string.IsNullOrEmpty(request.ImageUrl.ToString()))
                return;
            var existingImage = instructor.ImageUrl;
            var imageName = GetImageName(existingImage);
            if (!string.IsNullOrEmpty(existingImage) && existingImage != request.ImageUrl.ToString())
            { bunnyClient.DeleteFileAsync(imageName, Global.InstructorFolderName).Wait(); }
            instructor.ImageUrl = request.ImageUrl.ToString();
        }
        private string GetImageName(string url) => url.Split('/').Last();



        private void UploadNewImage(ref Instructor instructor,
            UpdateInstructorCommand request,
            BunnyClient bunnyClient)
        {
            if (string.IsNullOrEmpty(request.ImageUrl.ToString())) return;
            var newImageName = $"{instructor.InstructorId}_{GetImageName(request.ImageUrl.ToString())}";
            var response = bunnyClient.UploadFileAsync(request.ImageUrl, newImageName, Global.InstructorFolderName).Result;

            if (response.IsSuccessful && response.Url != null)
            { instructor.ImageUrl = response.Url; }



            if (!response.IsSuccessful || response.Url == null)
            {
                logger.LogWarning(
                    "Failed to upload image for Instructor {InstructorName}. Error: {Message}",
                    request.Name,
                    response.Message ?? ""
                );
            }




        }



    }
}

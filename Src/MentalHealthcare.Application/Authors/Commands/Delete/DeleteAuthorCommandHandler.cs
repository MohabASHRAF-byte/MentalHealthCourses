using MediatR;
using MentalHealthcare.Application.Advertisement.Commands.Delete;
using MentalHealthcare.Application.BunnyServices;
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

namespace MentalHealthcare.Application.Authors.Commands.Delete
{
    public class DeleteAuthorCommandHandler(
    ILogger<DeleteAuthorCommandHandler> logger,
    IAuthorRepository auRepo,
    IConfiguration configuration,
    IUserContext userContext
    ) : IRequestHandler<DeleteAuthorCommand>
    {
        public async Task Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
        {logger.LogInformation("Delete Author");
            var currentUser = userContext.GetCurrentUser();
            if (currentUser == null || !currentUser.HasRole(UserRoles.Admin))
            {logger.LogWarning("Unauthorized attempt to delete Author by user: {UserId}", currentUser?.Id);
                throw new ForBidenException("Don't have the permission to delete Author.");}
            var Au = await auRepo.GetAuthorById(request.AuthorID);
            var bunny = new BunnyClient(configuration);
            var imgName = GetImageName(Au.ImageUrl);
            await bunny.DeleteFileAsync(imgName, Global.AuthorFolderName);
            await auRepo.DeleteAuthorAsync(request.AuthorID);}

 private string GetImageName(string url)
        {
            return url.Split('/').Last();
        }




    }
}

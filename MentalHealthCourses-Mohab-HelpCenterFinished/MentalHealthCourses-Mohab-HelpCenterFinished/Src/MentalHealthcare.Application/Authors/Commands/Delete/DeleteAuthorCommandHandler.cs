using MediatR;
using MentalHealthcare.Application.Advertisement.Commands.Delete;
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
namespace MentalHealthcare.Application.Authors.Commands.Delete
{public class DeleteAuthorCommandHandler(
    ILogger<DeleteAuthorCommandHandler> logger,
    IAuthorRepository authorRepository,
    IConfiguration configuration
    ) : IRequestHandler<DeleteAuthorCommand>
    {public async Task Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
        {logger.LogInformation("Delete Author");
         var AU = await authorRepository.GetAuthorById(request.AuthorId);
         if (AU is null) { logger.LogWarning("Selected Author Not Existed !!"); }
         else { var bunny = new BunnyClient(configuration);
                var imgName = AU.ImageUrl;
                await bunny.DeleteFile(imgName, Global.AdvertisementFolderName);
                 await authorRepository.DeleteAuthor(request.AuthorId);}}}}


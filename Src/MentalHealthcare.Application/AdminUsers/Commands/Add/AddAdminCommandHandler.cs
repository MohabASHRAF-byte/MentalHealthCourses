using MediatR;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace MentalHealthcare.Application.AdminUsers.Commands.Add;

public class AddAdminCommandHandler(
    ILogger<AddAdminCommandHandler> logger,
    IAdminRepository adminRepository,
    IUserContext userContext
) : IRequestHandler<AddAdminCommand>
{
    [SuppressMessage("ReSharper.DPA", "DPA0006: Large number of DB commands", MessageId = "count: 27605")]
    public async Task Handle(AddAdminCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null || !currentUser.HasRole(UserRoles.Admin))
        {
            throw new ForBidenException($"Don't have the permission to add admin users.");
        }

        var admin = await adminRepository.GetAdminByIdentityAsync(currentUser.Id);
        if (admin == null)
            throw new ForBidenException("Admin with given identity does not exist.");
        var adminId = admin.AdminId;
        if (await adminRepository.IsPendingExistAsync(request.Email))
            throw new AlreadyExist($"Email {request.Email} already Pending.");

        if (await adminRepository.IsExistAsync(request.Email))
            throw new AlreadyExist($"Email {request.Email} already Administrator.");

        await adminRepository.AddPendingAsync(request.Email, adminId);
    }
}
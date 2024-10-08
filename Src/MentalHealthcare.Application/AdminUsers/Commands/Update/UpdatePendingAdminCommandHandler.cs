using MediatR;
using MentalHealthcare.Application.AdminUsers.Commands.Add;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.AdminUsers.Commands.Update;

public class UpdatePendingAdminCommandHandler(
    ILogger<AddAdminCommandHandler> logger,
    IAdminRepository adminRepository,
    IUserContext userContext
) : IRequestHandler<UpdatePendingAdminCommand>
{
    public async Task Handle(UpdatePendingAdminCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null || !currentUser.HasRole(UserRoles.Admin))
        {
            throw new ForBidenException($"Don't have the permission to add admin users.");
        }

        var admin = await adminRepository.GetAdminByIdentityAsync(currentUser.Id);
        if(admin == null)
            throw new ForBidenException("Admin with given identity does not exist.");
        
        var adminId = admin.AdminId;
        var result = await adminRepository.UpdatePendingAsync(request.OldEmail, request.NewEmail, adminId);
        if (!result)
            throw new ResourceNotFound("Old Mail", request.OldEmail);
    }
}
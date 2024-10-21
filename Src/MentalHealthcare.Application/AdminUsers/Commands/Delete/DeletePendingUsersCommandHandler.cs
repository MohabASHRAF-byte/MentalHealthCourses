using MediatR;
using MentalHealthcare.Application.AdminUsers.Commands.Add;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.AdminUsers.Commands.Delete;

public class DeletePendingUsersCommandHandler(
    ILogger<AddAdminCommandHandler> logger,
    IAdminRepository adminRepository,
    IUserContext userContext
    ) : IRequestHandler<DeletePendingUsersCommand>
{
    public async Task Handle(DeletePendingUsersCommand request, CancellationToken cancellationToken)
    {
        //todo  add auth logic 
        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null || !currentUser.HasRole(UserRoles.Admin))
        {
            throw new ForBidenException($"Don't have the permission to add admin users.");
        }
        await adminRepository.DeletePendingAsync(request.PendingUsers);
    }
}
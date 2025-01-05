using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.ContactUs.Queries.GetById;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.ContactUs.Queries.GetAll;

public class GetAllContactFormsQueryHandler(
    ILogger<GetContactFormByIdQueryHandler> logger,
    IContactUsRepository dbRepository,
    IMapper mapper,
    IUserContext userContext
) : IRequestHandler<GetAllContactFormsQuery, PageResult<ContactUsForm>>
{
    public async Task<PageResult<ContactUsForm>> Handle(GetAllContactFormsQuery request,
        CancellationToken cancellationToken)
    {
        userContext.EnsureAuthorizedUser([UserRoles.Admin], logger);
        var forms = await dbRepository.GetAllFormsAsync(request.PageNumber, request.PageSize,
            request.ViewMsgLengthLimiter, request.SenderName,
            request.SenderEmail, request.SenderPhone, request.IsRead);
        var contactUsForms = forms.Item2;
        var count = forms.Item1;
        return new PageResult<ContactUsForm>(
            contactUsForms,
            count,
            request.PageSize,
            request.PageNumber
        );
    }
}
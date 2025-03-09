using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.HomeScreenItems.Queries.GetAdvertisements;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Repositories;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.HomeScreenItems.Queries.GetRecentlyAddedCourses
{
    public class GetRecentlyAddedCoursesQueryHandler(
    ILogger<GetRecentlyAddedCoursesQueryHandler> logger,
    IHomeScreenRepository homeScreenRepository,
    IMapper mapper,
    IUserContext userContext
) : IRequestHandler<GetRecentlyAddedCoursesQuery, IEnumerable<CourseViewDto>>
    {
        public async Task<IEnumerable<CourseViewDto>> Handle(GetRecentlyAddedCoursesQuery request, CancellationToken cancellationToken)
        {




 logger.LogInformation("Handling GetAllAdvertisementsQuery with Count: {Count}",
                      request.Count);

            // Authorize user
            logger.LogInformation("Authorizing user for retrieving all Counts.");
            var currentUser = userContext.EnsureAuthorizedUser(new List<UserRoles> { UserRoles.User }, logger);
            logger.LogInformation("User {UserId} authorized to retrieve all Counts.", currentUser.Id);


            return await homeScreenRepository.GetRecentlyAddedCoursesAsync(request.Count);









        }
    }
}




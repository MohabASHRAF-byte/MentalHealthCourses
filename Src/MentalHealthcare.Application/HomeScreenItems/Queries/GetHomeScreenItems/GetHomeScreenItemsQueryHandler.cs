using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.Instructors.Commands.Update;
using MentalHealthcare.Application.Resources.Localization.Resources;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.HomeScreenItems.Queries.GetHomeScreenItems
{
    public class GetHomeScreenItemsQueryHandler(
        ILogger<GetHomeScreenItemsQueryHandler> logger,
        IMapper mapper,
        IHomeScreenRepository homRepo,
        IConfiguration configuration,
        IUserContext userContext,
        ILocalizationService localizationService
    ) : IRequestHandler<GetHomeScreenItemsQuery, PageResult<HomeScreenDto>>
    {

     
        //public async Task<HomeScreenDto> Handle(GetHomeScreenItemsQuery request, CancellationToken cancellationToken)
        //{


        //    // Retrieve the current user
        //    var currentUser = userContext.EnsureAuthorizedUser([UserRoles.User], logger);

          


        //    return await homRepo.GetHomeScreenDataAsync(
        //        request.instructorCount,
        //        request.advertisementCount,
        //        request.recentlyAddedCoursesCount);
        //}

       async Task<PageResult<HomeScreenDto>> IRequestHandler<GetHomeScreenItemsQuery, PageResult<HomeScreenDto>>.Handle(GetHomeScreenItemsQuery request, CancellationToken cancellationToken)
        {
            // Retrieve the current user
            var currentUser = userContext.EnsureAuthorizedUser([UserRoles.User], logger);

            // Fetch home screen data
            var homeScreenData = await homRepo.GetHomeScreenDataAsync(
                request.instructorCount,
                request.advertisementCount,
                request.recentlyAddedCoursesCount
            );

            // Wrap the result in PageResult
            return new PageResult<HomeScreenDto>(
                new List<HomeScreenDto> { homeScreenData }, // Convert to list
                totalCount: 1,  // Adjust based on actual data if needed
                pageSize: 1,    // Adjust based on actual pagination logic
                pageNumber: 1   // Assuming first page
            );
        }
    }
    }


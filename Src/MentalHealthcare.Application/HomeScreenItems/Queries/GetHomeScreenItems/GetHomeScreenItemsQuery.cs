using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.HomeScreenItems.Queries.GetHomeScreenItems
{
    public class GetHomeScreenItemsQuery : IRequest<PageResult<HomeScreenDto>>
    {

        public int instructorCount { get; set; }
        public int advertisementCount { get; set; }
        public int recentlyAddedCoursesCount { get; set; }
    }   
}


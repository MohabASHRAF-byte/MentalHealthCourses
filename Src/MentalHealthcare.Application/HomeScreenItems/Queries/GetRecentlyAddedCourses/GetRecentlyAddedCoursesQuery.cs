using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.HomeScreenItems.Queries.GetRecentlyAddedCourses
{
    public class GetRecentlyAddedCoursesQuery : IRequest<IEnumerable<CourseViewDto>>
    {
        public int Count { get; set; }

    }
}

using MentalHealthcare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Domain.Dtos
{
    public class HomeScreenDto
    {
        public IEnumerable<AdvertisementDto> Advertisements { get; set; }
        public int AdvertisementsTotalCount { get; set; }  // ✅ Added total count

        public IEnumerable<InstructorDto> Instructors { get; set; }
        public int InstructorsTotalCount { get; set; }  // ✅ Added total count

        public IEnumerable<CourseViewDto> RecentlyAddedCourses { get; set; }

        public IEnumerable<ArticleDto> Articles { get; set; }
        public int ArticlesTotalCount { get; set; }  // ✅ Added total count
    }
}

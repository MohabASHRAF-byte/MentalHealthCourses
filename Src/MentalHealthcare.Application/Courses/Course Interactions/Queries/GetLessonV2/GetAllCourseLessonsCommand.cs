using MediatR;
using MentalHealthcare.Domain.Dtos.course;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Courses.Course_Interactions.Queries.GetLessonV2
{
    public class GetAllCourseLessonsCommand : IRequest<List<CourseLessonDto>>
    {


        public int CourseId { get; set; }




    }

}

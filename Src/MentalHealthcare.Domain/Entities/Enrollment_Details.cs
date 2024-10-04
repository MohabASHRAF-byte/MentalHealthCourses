using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Domain.Entities
{
    public class Enrollment_Details
    {

        public DateTime Date { get; set; }
        public decimal Progress { get; set; }
        public int StudentID { get; set; }
        public int CourseID { get; set; }


        public User users { get; set; } = default!;
        public Course Courses { get; set; } = default!;
    

    }
}

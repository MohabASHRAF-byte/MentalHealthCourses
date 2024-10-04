using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Domain.Entities
{
    public class Instructor : ContentCreatorBE
    {

        public int InstructorId { get; set; }

        public List<Course>? courses { get; set; }


    }
}

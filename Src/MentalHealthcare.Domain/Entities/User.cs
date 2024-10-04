using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Domain.Entities
{
    public class User : HumanBE
    {
        public int UserId { get; set; }
        public DateTime DOF { get; set; }


  

        public List<Course>? Fav_Courses { get; set; } = new();

        public List<Course>? Course_Rates { get; set; } = new();

        public List<Logs>? Logs { get; set; } = new();
        public List<Payments>? Payments { get; set; } = new();

    }
}

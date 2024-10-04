using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Domain.Entities
{
    public class Category
    {


        public int CategoryID { get; set; }
        public string Name { get; set; }

        public string? description { get; set; }



        public ICollection<Course> Courses { get; set; }
        = new HashSet<Course>();    


    }
}

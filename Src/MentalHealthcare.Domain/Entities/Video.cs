using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Domain.Entities
{
    public class Video : MaterialBE
    {

        public int VideoId { get; set; }

        public string ThumbnailUrl { get; set; }


        public Course? Courses { get; set; }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Domain.Entities
{
    public class Author : ContentCreatorBE
    {

        public int AuthorId { get; set; }

        public List<Article> Articles { get; set; }


    }
}

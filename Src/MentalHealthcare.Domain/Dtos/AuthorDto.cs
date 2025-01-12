using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Domain.Dtos
{
    public class AuthorDto
    {
        public int AuthorId { get; set; }
        public string Name { get; set; }
        public string? About { get; set; }
        public string? ImageUrl { get; set; }
        public List<string> Articles { get; set; } = [];
    }
}

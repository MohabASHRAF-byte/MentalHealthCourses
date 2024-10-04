using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Domain.Entities
{
    public class ContentCreatorBE
    {

        public string About { get; set; }
        public string Name { get; set; }
        public byte[]? Image { get; set; }

        public Admin Admins { get; set; } = default!;


    }
}

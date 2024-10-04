using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Domain.Entities
{
    public class Payments
    {
        public string Type { get; set; }
        public long Card_Number { get; set; }

        public DateTime month { get; set; }
        public DateOnly Year { get; set; }
        public string Name { get; set; }

        public User users { get; set; }

    }
}

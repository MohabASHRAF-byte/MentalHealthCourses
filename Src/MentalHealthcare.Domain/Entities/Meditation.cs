using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Domain.Entities
{
    public class Meditation : MaterialBE
    {
        public int MeditationId { get; set; }
        
        public string Content { get; set; }













    }
}

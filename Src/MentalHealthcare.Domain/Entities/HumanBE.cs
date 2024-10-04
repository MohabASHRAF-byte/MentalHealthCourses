using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Domain.Entities
{
    public class HumanBE
    {
  [Key]
 [DataType(DataType.EmailAddress)]
public string Gmail { get; set; }


        [Required]
        public string FName { get; set; }
        [Required]
        public string LName { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)] 
        public string PhoneNumber { get; set; }

        [DataType(DataType.Password)]
        public string Passwprd { get; set; }








    }
}

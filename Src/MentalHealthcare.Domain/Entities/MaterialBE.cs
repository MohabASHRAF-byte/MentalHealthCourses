using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Domain.Entities
{
    public class MaterialBE
    {
        public string Url { get; set; }
        public string Title { get; set; }
        public String UploadedBy { get; set; }
        public DateTime UploadDate{ get; set; }

        [NotMapped]
        // This indicates that this property should not be mapped to the database
        public DateTime CreatedDate => UploadDate;


        public Admin Admins { get; set; } = default!;






    }
}

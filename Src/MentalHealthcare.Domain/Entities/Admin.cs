using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Domain.Entities
{
    public class Admin : HumanBE
    {        public int AdminId { get; set; }


        #region  Navigation property => Many

        public List<Article> articles { get; set; } = new();
        public List<Podcast> podcasts { get; set; } = new();

        public List<Meditation> Meditations { get; set; } = new();


        public List<Pdf> pdfs { get; set; } = new();

        public List<Video> videos { get; set; } = new(); 
        #endregion
    }
}

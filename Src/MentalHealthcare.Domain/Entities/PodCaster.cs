﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Domain.Entities
{
    public class PodCaster : ContentCreatorBE
    {

        public int PodCasterId { get; set; }

        public List<Podcast> Podcasts { get; set; }
    }
}
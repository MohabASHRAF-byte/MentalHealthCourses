using MediatR;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Podcasts.Commands.Create
{
    public class AddPodcastCommand : IRequest<int>
    {


  [MaxLength(1000)] public List<IFormFile> PodcastDescription { get; set; } = [];  
 [MaxLength(Global.MaxNameLength)]public string Name { get; set; } = default!;
        public string? About { get; set; }
        public int PodcastLength { get; set; }
        public PodCaster PodCaster { get; set; } = default!;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;


    }
}

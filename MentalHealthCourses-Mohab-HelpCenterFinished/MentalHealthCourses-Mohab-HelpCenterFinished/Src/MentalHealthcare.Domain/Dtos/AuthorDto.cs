using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Domain.Dtos
{public class AuthorDto
    {public int AuthorId { get; set; }
     public string Name { get; set; } = default!;
     public string? ImageUrl { get; set; }
     public string? About { get; set; }
     public Admin AddedBy { get; set; } = default!;
     public List<ArticleDto> Articles { get; set; } = new();}}

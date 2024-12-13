using AutoMapper;
using MentalHealthcare.Application.Articles.Commands.AddArticle;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Articles
{
    public class ArticleProfile : Profile
    {
        public ArticleProfile()
        {{ CreateMap<Article, ArticleDto>()
     .ForMember(d => d.Author, o => o.MapFrom(s => s.Author.Name )).
     ForMember(d => d.UploadedBy, o=> o.MapFrom(src => src.UploadedBy.FName ));


        }


        }

    }
}
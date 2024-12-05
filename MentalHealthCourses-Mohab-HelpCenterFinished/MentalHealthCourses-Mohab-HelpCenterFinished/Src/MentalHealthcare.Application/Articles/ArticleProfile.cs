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
        {

            { // Mapping from Article entity to ArticleDto
                CreateMap<Domain.Entities.Article, ArticleDto>()
                       .ForMember(Ar => Ar.AuthorName, Ti => Ti.MapFrom(c => c.Author.Name));
                // Optionally map ArticleDto back to Article
                CreateMap<ArticleDto, Domain.Entities.Article>().ForMember(L => L.Author, e=> e.Ignore());
                // Mapping from CreateArticleCommand to Article entity
                CreateMap<AddArticleCommand, Domain.Entities.Article>().ReverseMap();




            }


        }

    }
}
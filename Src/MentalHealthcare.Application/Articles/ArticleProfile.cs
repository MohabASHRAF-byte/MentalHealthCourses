using AutoMapper;
using MentalHealthcare.Application.Advertisement.Commands.Create;
using MentalHealthcare.Application.Articles.Commands.Ctreate;
using MentalHealthcare.Domain.Dtos;
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
            CreateMap<Domain.Entities.Article, ArticleDto>()
            .ForMember(dest => dest.ArticleImageUrls,
                opt => opt.MapFrom(src => src.ArticleImageUrls.Select(img => img.ImageUrl).ToList()));

            // Optionally map AdvertisementDto back to  Article
            CreateMap<ArticleDto, Domain.Entities.Article>()
                .ForMember(dest => dest.ArticleImageUrls,
                    opt => opt.Ignore());
            CreateMap<CreateArticleCommand, Domain.Entities.Article>().ReverseMap();





            CreateMap<Domain.Entities.Article, ArticleDto>()
            .ForMember(dest => dest.AuthorName,
                opt => opt.MapFrom(src => src.Author.Name));

            // Optionally map AdvertisementDto back to  Article
            CreateMap<ArticleDto, Domain.Entities.Article>()
                .ForMember(dest => dest.Author,
                    opt => opt.Ignore());
            CreateMap<CreateArticleCommand, Domain.Entities.Article>().ReverseMap();




        }






    }
}

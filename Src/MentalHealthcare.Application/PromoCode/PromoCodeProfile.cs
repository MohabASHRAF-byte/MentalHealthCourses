using AutoMapper;
using MentalHealthcare.Application.PromoCode.Course;
using MentalHealthcare.Application.PromoCode.Course.Commands.AddCoursePromoCode;
using MentalHealthcare.Application.PromoCode.General.Commands.AddGeneralPromoCode;
using MentalHealthcare.Domain.Dtos.PromoCode;
using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Application.PromoCode;

public class PromoCodeProfile : Profile
{
    public PromoCodeProfile()
    {
        CreateMap<AddCoursePromoCodeCommand, CoursePromoCode>().ReverseMap();
        CreateMap<CoursePromoCode, CoursePromoCodeDto>().ReverseMap();
        CreateMap<AddGeneralPromoCodeCommand, GeneralPromoCode>().ReverseMap();
    }
}
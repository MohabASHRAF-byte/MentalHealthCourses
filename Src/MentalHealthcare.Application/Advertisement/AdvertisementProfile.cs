using AutoMapper;
using MentalHealthcare.Application.Advertisement.Commands.Create;
using MentalHealthcare.Domain.Dtos;

namespace MentalHealthcare.Application.Advertisement;

public class AdvertisementProfile : Profile
{
    public AdvertisementProfile()
    {
        CreateMap<Domain.Entities.Advertisement, AdvertisementDto>()
            .ForMember(dest => dest.AdvertisementImageUrls,
                opt => opt.MapFrom(src => src.AdvertisementImageUrls.Select(img => img.ImageUrl).ToList()));

        // Optionally map AdvertisementDto back to Advertisement
        CreateMap<AdvertisementDto, Domain.Entities.Advertisement>()
            .ForMember(dest => dest.AdvertisementImageUrls,
                opt => opt.Ignore());
        CreateMap<CreateAdvertisementCommand, Domain.Entities.Advertisement>().ReverseMap();
    }
}
using AutoMapper;
using MentalHealthcare.Application.Meditations.Command.Create;
using MentalHealthcare.Application.Meditations.Command.Update;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Application.Meditations
{
    public class MeditationProfile : Profile
    {
        public MeditationProfile()
        {
            // Mapping for creating a meditation
            CreateMap<CreateMeditationCommand, Meditation>()
                .ForMember(dest => dest.Image, opt => opt.Ignore()); // Ignore image URL, it's set separately

            // Mapping for updating a meditation
            CreateMap<UpdateMeditationCommand, Meditation>()
                .ForMember(dest => dest.Image, opt => opt.Ignore());

            CreateMap<Meditation, MeditationDto>();


            // Ignore image URL to prevent overwriting unless explicitly updated
        }
    }
}

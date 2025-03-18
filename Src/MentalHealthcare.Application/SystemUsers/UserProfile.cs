using AutoMapper;
using MentalHealthcare.Application.SystemUsers.Commands.Register;
using MentalHealthcare.Domain.Dtos.User;
using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Application.SystemUsers;

public class UserProfile:AutoMapper.Profile
{
    public UserProfile()
    {
        CreateMap<RegisterCommand, User>()
            .ForMember(u => u.TwoFactorEnabled, opt => opt.MapFrom(c => c.Active2Fa));
        CreateMap<RegisterCommand, SystemUser>();
        CreateMap<RegisterCommand, UserDto>();

        CreateMap<SystemUser, UserProfileDto>();
        CreateMap<SystemUser, UserProfileDto>()
            .ForMember(dest => dest.ProfilePic, opt => opt.MapFrom(src =>
                string.IsNullOrEmpty(src.ProfilePic)
                ? null
                : $"https://pullzonegeneralcommittee.b-cdn.net/ProfilePics/{src.ProfilePic}"));
    }

}
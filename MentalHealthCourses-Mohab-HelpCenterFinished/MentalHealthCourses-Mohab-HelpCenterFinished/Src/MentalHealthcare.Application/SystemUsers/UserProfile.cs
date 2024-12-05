using AutoMapper;
using MentalHealthcare.Application.SystemUsers.Commands.Register;
using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Application.SystemUsers;

public class UserProfile:Profile
{
    public UserProfile()
    {
        CreateMap<RegisterCommand, User>()
            .ForMember(u => u.TwoFactorEnabled, opt => opt.MapFrom(c => c.Active2Fa));
        CreateMap<RegisterCommand, SystemUser>();
        CreateMap<RegisterCommand, UserDto>();
    }
}
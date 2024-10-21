using AutoMapper;
using MentalHealthcare.Application.AdminUsers.Commands.Register;
using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Application.AdminUsers;

public class PendingAdminProfile : Profile
{
    public PendingAdminProfile()
    {
        CreateMap<PendingUsersDto, PendingAdmins>().ReverseMap();
        CreateMap<RegisterAdminCommand, User>()
            .ForMember(u => u.TwoFactorEnabled, opt => opt.MapFrom(c => c.Active2Fa)).ReverseMap();
    }
}
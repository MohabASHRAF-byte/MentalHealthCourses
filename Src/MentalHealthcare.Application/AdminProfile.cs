using AutoMapper;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Application;

public class AdminProfile : Profile
{
    public AdminProfile()
    {
        CreateMap<User, UserDto>();
    }
}
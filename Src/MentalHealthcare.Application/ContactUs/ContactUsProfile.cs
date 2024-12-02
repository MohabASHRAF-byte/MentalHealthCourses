using AutoMapper;
using MentalHealthcare.Application.ContactUs.Commands.Create;
using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Application.ContactUs;

public class ContactUsProfile:Profile
{
    public ContactUsProfile()
    {
        CreateMap<SubmitContactUsCommand, ContactUsForm>().ReverseMap();
    }
}
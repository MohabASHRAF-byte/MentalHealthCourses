using AutoMapper;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos.OrderProcessing;
using MentalHealthcare.Domain.Entities.OrderProcessing;

namespace MentalHealthcare.Application.OrderProcessing;

public class OrderProcessingProfile : Profile
{
    public OrderProcessingProfile()
    {
        // Map CourseCartDto to CourseOrderView
        CreateMap<CourseCartDto, CourseOrderView>();

        // Map CartDto to Invoice
        CreateMap<CartDto, Invoice>()
            .ForMember(dest => dest.Courses, opt => opt.MapFrom(src => src.Courses)) // Map Courses
            .ForMember(dest => dest.InvoiceId, opt => opt.Ignore()) // Ignore OrderId (it's generated separately)
            .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(_ => DateTime.UtcNow)) // Set current date
            .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(_ => OrderStatus.Pending)); // Default status
    }
}
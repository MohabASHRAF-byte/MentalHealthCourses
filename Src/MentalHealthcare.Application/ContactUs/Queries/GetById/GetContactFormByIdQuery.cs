using MediatR;
using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Application.ContactUs.Queries.GetById;

public class GetContactFormByIdQuery:IRequest<ContactUsForm>
{
    public int Id { get; set; }
}
using System.Text.Json.Serialization;
using MediatR;
using MentalHealthcare.Domain.Dtos.course;

namespace MentalHealthcare.Application.OrderProcessing.Order.Commands.Accept_Invoice;


public class AcceptInvoiceCommand : IRequest
{
    [JsonIgnore]
    public int InvoiceId { get; set; }
    
    public List<MiniCourse> Courses { get; set; } = new ();
    
    public decimal Discount { get; set; }
    
    
}
using MediatR;

namespace MentalHealthcare.Application.ContactUs.Commands.Change_Read_state;

public class ChangeReadStateCommand:IRequest
{
    public int FormId { get; set; }
    public bool State { get; set; }
}
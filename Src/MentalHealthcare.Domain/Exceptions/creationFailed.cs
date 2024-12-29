namespace MentalHealthcare.Domain.Exceptions;

public class CreationFailed(string msg) : Exception(msg)
{
}
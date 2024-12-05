namespace MentalHealthcare.Domain.Exceptions;

public class ResourceNotFound(string type, string id)
    : Exception($"No {type} with Id : {id} exists. ")
{
}
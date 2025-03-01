namespace MentalHealthcare.Domain.Exceptions;

public class ResourceNotFound(string type, string id, string v)
    : Exception($"No {type} with Id : {id} exists. ")
{
}
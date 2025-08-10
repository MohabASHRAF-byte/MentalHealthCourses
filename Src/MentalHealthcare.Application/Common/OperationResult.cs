// Enhanced OperationResult Class


using MentalHealthcare.Domain.Constants;

namespace MentalHealthcare.Application.Common;

public class OperationResult<T>
{
    public bool Success { get; set; }
    public StateCode StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<string> Errors { get; set; } = new List<string>();
    public T? Data { get; set; } = default(T);


    public static OperationResult<T> SuccessResult(
        T data,
        string message = "",
        StateCode statusCode = StateCode.Ok)
    {
        return new OperationResult<T>
        {
            Data = data,
            Success = true,
            Message = message,
            StatusCode = statusCode
        };
    }

    public static OperationResult<T> Failure(string message, StateCode statusCode = StateCode.BadRequest)
    {
        return new OperationResult<T>
        {
            Success = false,
            Message = message,
            StatusCode = statusCode
        };
    }


    public static OperationResult<T> Failure(IEnumerable<string> errors, StateCode code = StateCode.BadRequest)
    {
        var errorList = errors.ToList();
        var combinedMessage = string.Join(" | ", errorList); // or use "\n" for line breaks

        return new OperationResult<T>
        {
            Success = false,
            StatusCode = code,
            Message = combinedMessage,
            Errors = errorList,
            Data = default(T)
        };
    }







}


/*
 
 
 
 
 
 
 
 
 */
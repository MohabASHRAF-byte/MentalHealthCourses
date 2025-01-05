using System.Net;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Exceptions;
using Microsoft.ApplicationInsights.Extensibility.Implementation;

namespace MentalHealthcare.API.MiddleWares;

public class GlobalErrorHandling(
    ILogger<GlobalErrorHandling> logger
) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (AlreadyExist ex)
        {
            logger.LogError(ex, ex.Message);
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync(ex.Message);
        }
        catch (ResourceNotFound ex)
        {
            logger.LogError(ex, "Resource not found: {Message}", ex.Message);
            context.Response.StatusCode = 404;
            var ret = OperationResult<string>.Failure(ex.Message, statusCode: StateCode.NotFound);
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(ret);
        }
        catch (ForBidenException ex)
        {
            logger.LogError(ex, "Forbiden: {Message}", ex.Message);
            context.Response.StatusCode = 401;
            var ret = OperationResult<string>.Failure(ex.Message, statusCode: StateCode.Forbidden);
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(ret);
        }
        catch (ArgumentException ex)
        {
            logger.LogError(ex, "Argument: {Message}", ex.Message);
            context.Response.StatusCode = 400;
            var ret = OperationResult<string>.Failure(ex.Message, statusCode: StateCode.BadRequest);
            ret.Errors.Add(ex.Message);
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(ret);
        }
        catch (CreationFailed ex)
        {
            logger.LogError(ex, "Argument: {Message}", ex.Message);
            context.Response.StatusCode = 400;
            var ret = OperationResult<string>.Failure(ex.Message, statusCode: StateCode.BadRequest);
            ret.Errors.Add(ex.Message);
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(ret);
        }catch (InvalidOtp ex)
        {
            logger.LogError(ex, "otp is invalid");
            context.Response.StatusCode = 400;
            var ret = OperationResult<string>.Failure("Otp is invalid", statusCode: StateCode.BadRequest);
            ret.Errors.Add(ex.Message);
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(ret);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("An unexpected error occurred.");
        }
    }
}
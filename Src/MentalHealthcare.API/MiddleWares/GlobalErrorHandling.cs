using MentalHealthcare.Domain.Exceptions;

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
            logger.LogError(ex, ex.Message);
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync(ex.Message);
        }
        catch (ForBidenException ex)
        {
            logger.LogError(ex, ex.Message);
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("An unexpected error occurred.");
        }
    }
}
using System.Diagnostics;

namespace MentalHealthcare.API.MiddleWares;

public class RequestTimeLogging(
    ILogger<RequestTimeLogging> logger
) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        await next.Invoke(context);
        stopwatch.Stop();
        var time = stopwatch.ElapsedMilliseconds > 0;
        if (time)
            logger.LogWarning("Request [{verb}] \n at {path} took time: {time} ms"
                , context.Request.Method, context.Request.Path, stopwatch.ElapsedMilliseconds
                );
    }
}
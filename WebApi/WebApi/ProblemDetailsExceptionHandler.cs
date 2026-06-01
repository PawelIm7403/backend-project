using CoreApp.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace WebApi.WebApi;

public class ProblemDetailsExceptionHandler(
    ProblemDetailsFactory factory,
    ILogger<ProblemDetailsExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is GateNotFoundException)
        {
            logger.LogInformation("Exception '{Message}' handled!", exception.Message);

            var problem = factory.CreateProblemDetails(
                context,
                StatusCodes.Status400BadRequest,
                "Service error!",
                "Service error",
                detail: exception.Message
            );

            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            await context.Response.WriteAsJsonAsync(problem, cancellationToken);

            return true;
        }

        return false;
    }
}
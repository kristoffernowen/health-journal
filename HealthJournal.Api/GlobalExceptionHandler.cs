using HealthJournal.Api.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace HealthJournal.Api;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {

        ProblemDetails problem;

        if (exception is DomainException domain)
        {
            problem = new ProblemDetails
            {
                Title = "Domain error",
                Detail = domain.Message,
                Status = domain.StatusCode
            };

            httpContext.Response.StatusCode = domain.StatusCode;
        }
        else
        {
            logger.LogError(exception, "An unexpected error occurred.");

            problem = new ProblemDetails
            {
                Title = "An unexpected error occurred",
                Status = StatusCodes.Status500InternalServerError
            };

            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        }

        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);
        return true;
    }
}

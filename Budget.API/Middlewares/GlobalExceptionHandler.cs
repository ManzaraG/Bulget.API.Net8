using Budget.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Budget.API.Middlewares;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var (statusCode, title) = exception switch
        {
            NotFoundException => (StatusCodes.Status404NotFound, "Ressource introuvable"),
            ForbiddenAccessException => (StatusCodes.Status403Forbidden, "Accès refusé"),
            ConflictException => (StatusCodes.Status409Conflict, "Conflit"),
            ArgumentException => (StatusCodes.Status400BadRequest, "Requête invalide"),
            _ => (StatusCodes.Status500InternalServerError, "Erreur interne"),
        };

        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsJsonAsync(
            new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = exception.Message,
            },
            cancellationToken);

        return true;
    }
}

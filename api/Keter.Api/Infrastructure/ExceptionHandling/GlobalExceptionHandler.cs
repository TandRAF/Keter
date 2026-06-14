// Keter.Api/Infrastructure/ExceptionHandling/GlobalExceptionHandler.cs
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Keter.Api.Infrastructure.ExceptionHandling;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, 
        Exception exception, 
        CancellationToken cancellationToken)
    {
        // Verificăm dacă excepția este cea aruncată de ValidationBehavior
        if (exception is ValidationException validationException)
        {
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

            // Creăm un dicționar cu erorile pentru a le mapa ușor în React
            var errors = validationException.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key, 
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );

            // Folosim standardul RFC 7807 (Problem Details) pentru răspunsuri HTTP
            var problemDetails = new ValidationProblemDetails(errors)
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation Failed",
                Detail = "One or more validation errors occurred."
            };

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            
            return true; // Spunem framework-ului că am rezolvat noi excepția
        }

        // Dacă e altă eroare (ex: baza de date picată), o lăsăm să treacă mai departe
        return false; 
    }
}
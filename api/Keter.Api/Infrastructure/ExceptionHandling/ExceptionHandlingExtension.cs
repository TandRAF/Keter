// Keter.Api/Infrastructure/Extensions/ExceptionHandlingExtension.cs
using Keter.Api.Infrastructure.ExceptionHandling;

namespace Keter.Api.Infrastructure.Extensions;

public static class ExceptionHandlingExtension
{
    public static IServiceCollection AddKeterExceptionHandling(this IServiceCollection services)
    {
        // Înregistrăm handler-ul nostru global
        services.AddExceptionHandler<GlobalExceptionHandler>();
        
        // Adăugăm suportul standard pentru ProblemDetails în .NET
        services.AddProblemDetails();

        return services;
    }
}
// Keter.Api/Infrastructure/Behaviors/ValidationBehavior.cs
using FluentValidation;
using MediatR;

namespace Keter.Api.Infrastructure.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    // Injectăm toate validatoarele găsite în proiect pentru cererea curentă
    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next(); // Dacă nu există validatoare pentru comanda asta, mergem mai departe la Handler
        }

        var context = new ValidationContext<TRequest>(request);

        // Rulăm toate validatoarele în paralel
        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        // Colectăm toate erorile
        var failures = validationResults
            .Where(r => r.Errors.Any())
            .SelectMany(r => r.Errors)
            .ToList();

        if (failures.Any())
        {
            // Oprim fluxul și aruncăm excepția de la FluentValidation
            throw new ValidationException(failures);
        }

        // Dacă totul e valid, trecem la următorul pas (sau la Handler)
        return await next();
    }
}
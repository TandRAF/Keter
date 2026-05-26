// Keter.Api/Infrastructure/Behaviors/ValidationBehavior.cs
using FluentValidation;
using MediatR;

namespace Keter.Api.Infrastructure.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // If this request doesn't have any validators, just let it through
        if (!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        // Run all validators for this specific request
        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        // If any validation rules failed, throw an exception BEFORE hitting the handler!
        if (failures.Any())
        {
            throw new ValidationException(failures);
        }

        // Everything is valid. Let the request proceed to the Handler.
        return await next();
    }
}
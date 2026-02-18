using Domain.Shared;
using FluentValidation;
using MediatR;
using System.Reflection;

namespace Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : class
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failure = validationResults
            .SelectMany(r => r.Errors)
            .FirstOrDefault(f => f != null);

        if (failure != null)
        {
            return CreateFailedResult<TResponse>(failure.ErrorMessage);
        }

        return await next(cancellationToken);
    }

    private static TResult CreateFailedResult<TResult>(string message)
    {
        var type = typeof(TResult);

        var failureMethod = type.GetMethod("Failure",
            BindingFlags.Static | BindingFlags.Public);

        if (failureMethod is null)
        {
            throw new InvalidOperationException($"Type '{type.Name}' does not have a suitable Failure method.");
        }
        
        var result = failureMethod.Invoke(null, [message, ErrorType.Validation]);

        return (TResult)result!;
    }
}

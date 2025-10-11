﻿using FluentValidation;
using MediatR;
using Shared.Contracts.CQRS;

namespace Shared.Behaviors;
public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);
        var validationResults = 
            await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var errors = validationResults
            .SelectMany(validationResults => validationResults.Errors)
            .Where(error => error != null)
            .ToList();

        if (errors.Count > 0)
        {
            throw new ValidationException(errors);
        }

        return await next(cancellationToken);
    }
}

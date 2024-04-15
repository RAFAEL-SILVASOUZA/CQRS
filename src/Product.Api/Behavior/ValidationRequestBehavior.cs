using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Product.Api.Domain.Notifications;
using Product.Api.Domain.Responses;

namespace Product.Api.Behavior;

public class ValidationRequestBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse?>
    where TRequest : IRequest<IResponse>
    where TResponse : IResponse

{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly IDomainNotificationContext _notificationContext;

    public ValidationRequestBehavior(IEnumerable<IValidator<TRequest>> validators, IDomainNotificationContext notificationContext)
    {
        _validators = validators;
        _notificationContext = notificationContext;
    }

    public Task<TResponse?> Handle(TRequest request, RequestHandlerDelegate<TResponse?> next, CancellationToken cancellationToken)
    {
        var failures = _validators
           .Select(v => v.Validate(request))
           .SelectMany(x => x.Errors)
           .Where(f => f != null)
           .ToList();

        return failures.Any() ? Notify(failures) : next();
    }

    private Task<TResponse?> Notify(IEnumerable<ValidationFailure> failures)
    {
        var result = default(TResponse);

        foreach (var failure in failures)
        {
            _notificationContext.NotifyBadRequest(failure.ErrorMessage);
        }

        return Task.FromResult(result);
    }
}

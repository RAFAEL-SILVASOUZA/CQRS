using Amazon.Runtime.Internal;

namespace Product.Api.Domain.Notifications;

public class DomainNotificationContext : IDomainNotificationContext
{
    private readonly List<DomainNotification> _notifications;

    public DomainNotificationContext()
    {
        _notifications = new List<DomainNotification>();
    }

    public bool HasErrorNotifications
        => _notifications.Any();

    public void NotifyNotFound(string message)
    => Notify(message, DomainNotificationType.NotFound);

    public void NotifyBadRequest(string message)
    => Notify(message, DomainNotificationType.BadRequest);

    public void NotifyException(string message)
    => Notify(message, DomainNotificationType.Exception);

    private void Notify(string message, DomainNotificationType type)
        => _notifications.Add(new DomainNotification(type, message));

    public IResult GetErrorNotifications()
    {
        var firstError = _notifications.FirstOrDefault();
        var errors = _notifications.Select(x => x.Value);

        switch (firstError?.Type)
        {
            case DomainNotificationType.NotFound:
                return Results.NotFound(errors);
            case DomainNotificationType.BadRequest:
                return Results.BadRequest(errors);
            case DomainNotificationType.Exception:
                return Results.NotFound(errors);
            default:
                break;
        }

        return Results.Ok();
    }
}

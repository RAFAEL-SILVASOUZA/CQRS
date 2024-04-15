namespace Product.Api.Domain.Notifications;

public interface IDomainNotificationContext
{
    bool HasErrorNotifications { get; }    
    void NotifyNotFound(string message);
    void NotifyBadRequest(string message);
    void NotifyException(string message);
    IResult GetErrorNotifications();
}

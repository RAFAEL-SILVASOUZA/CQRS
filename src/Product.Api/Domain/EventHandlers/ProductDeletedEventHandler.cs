using DotNetCore.CAP;
using MediatR;
using Product.Api.Domain.Constants;
using Product.Api.Domain.Events;
using Product.Api.Infrastructure;

namespace Product.Api.Domain.EventHandlers;

public sealed class ProductDeletedEventHandler(IProductMongoContext productMongoContext, ICapPublisher capPublisher)
    : INotificationHandler<ProductDeletedEvent>, ICapSubscribe
{
    public Task Handle(ProductDeletedEvent notification, CancellationToken cancellationToken)
    => capPublisher.PublishAsync(Queues.PRODUCT_DELETED, notification.Id.ToString(), cancellationToken: cancellationToken);


    [CapSubscribe(Queues.PRODUCT_DELETED)]
    public Task TryHandle(string id)
    => productMongoContext.DeleteAsync(new Guid(id));
}

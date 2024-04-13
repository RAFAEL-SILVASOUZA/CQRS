using DotNetCore.CAP;
using MediatR;
using Product.Api.Domain.Constants;
using Product.Api.Domain.Entities;
using Product.Api.Domain.Events;
using Product.Api.Infrastructure;

namespace Product.Api.Domain.EventHandlers;

public class ProductUpdatedEventHandler(IProductMongoContext productMongoContext, ICapPublisher capPublisher)
    : INotificationHandler<ProductUpdatedEvent>, ICapSubscribe
{
    public Task Handle(ProductUpdatedEvent notification, CancellationToken cancellationToken)
    => capPublisher.PublishAsync(Queues.PRODUCT_UPDATED, notification.Product.SetId(notification.Id), cancellationToken: cancellationToken);


    [CapSubscribe(Queues.PRODUCT_UPDATED)]
    public Task TryHandle(BsonProduct product)
    => productMongoContext.UpdateAsync(new Guid(product.Id), product);
}

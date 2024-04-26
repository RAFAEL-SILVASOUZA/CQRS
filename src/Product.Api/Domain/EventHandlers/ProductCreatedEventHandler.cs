﻿using DotNetCore.CAP;
using MediatR;
using Product.Api.Domain.Constants;
using Product.Api.Domain.Events;
using Product.Api.Infrastructure.Data.ReadOnly;

namespace Product.Api.Domain.EventHandlers;

public sealed class ProductCreatedEventHandler(IProductMongoContext productMongoContext, ICapPublisher capPublisher)
    : INotificationHandler<ProductCreatedEvent>, ICapSubscribe
{
    public Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
    => capPublisher.PublishAsync(Queues.PRODUCT_CREATED, notification.Product, cancellationToken: cancellationToken);
}

using MediatR;
using Product.Api.Domain.Entities;

namespace Product.Api.Domain.Events;

public sealed record ProductUpdatedEvent(Guid Id, BsonProduct Product) : INotification;


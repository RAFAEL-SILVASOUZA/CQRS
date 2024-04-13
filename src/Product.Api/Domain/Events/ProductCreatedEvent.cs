using MediatR;
using Product.Api.Domain.Entities;

namespace Product.Api.Domain.Events;

public sealed record ProductCreatedEvent(BsonProduct Product) : INotification;

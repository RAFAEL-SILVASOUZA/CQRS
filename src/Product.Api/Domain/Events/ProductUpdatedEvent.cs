using MediatR;
using Product.Api.Domain.Entities;
using Product.Api.Domain.Responses;

namespace Product.Api.Domain.Events;

public sealed record ProductUpdatedEvent(Guid Id, BsonProduct Product) : INotification;


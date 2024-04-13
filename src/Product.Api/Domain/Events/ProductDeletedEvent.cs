using MediatR;

namespace Product.Api.Domain.Events;

public sealed record ProductDeletedEvent(Guid Id) : INotification;

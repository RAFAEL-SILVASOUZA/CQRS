using MediatR;

namespace Product.Api.Domain.Command;

public sealed record ProductDeleteCommand(Guid Id) : IRequest<Unit>;

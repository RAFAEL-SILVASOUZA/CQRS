using MediatR;
using Product.Api.Domain.Responses;

namespace Product.Api.Domain.Command;

public sealed record ProductUpdateCommand(Guid Id,string Description, double Price) : IRequest<ProductUpdatedResponse>;

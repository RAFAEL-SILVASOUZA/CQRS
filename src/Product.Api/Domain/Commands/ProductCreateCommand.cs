using MediatR;
using Product.Api.Domain.Responses;

namespace Product.Api.Domain.Command;

public sealed record ProductCreateCommand(string Description, double Price) : IRequest<ProductCreatedResponse>;


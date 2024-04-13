using MediatR;
using Product.Api.Domain.Responses;

namespace Product.Api.Domain.Queries;

public sealed record ProductByIdQuery(Guid Id) : IRequest<ProductQueryResponse?>;

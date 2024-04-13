using MediatR;
using Product.Api.Domain.Responses;

namespace Product.Api.Domain.Queries
{
    public sealed record ProductsByDescriptionQuery(string Description) : IRequest<IEnumerable<ProductQueryResponse>>;
}

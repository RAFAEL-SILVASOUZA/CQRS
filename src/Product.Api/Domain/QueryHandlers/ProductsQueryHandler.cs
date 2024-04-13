using MediatR;
using Product.Api.Domain.Queries;
using Product.Api.Domain.Responses;
using Product.Api.Infrastructure.ReadOnly;

namespace Product.Api.Domain.QueryHandlers;

public sealed class ProductsQueryHandler(IProductMongoContext productMongoContext) 
    : IRequestHandler<ProductsQuery, IEnumerable<ProductQueryResponse>>
{
    public async Task<IEnumerable<ProductQueryResponse>> Handle(ProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await productMongoContext.GetAsync();
        return from p in products
               select p.ToProductQueryResponse();
    }
}

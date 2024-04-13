using MediatR;
using Product.Api.Domain.Queries;
using Product.Api.Domain.Responses;
using Product.Api.Infrastructure.ReadOnly;

namespace Product.Api.Domain.QueryHandlers;

public sealed class ProductsByDescriptionQueryHandler(IProductMongoContext productMongoContext)
    : IRequestHandler<ProductsByDescriptionQuery, IEnumerable<ProductQueryResponse>>
{
    public async Task<IEnumerable<ProductQueryResponse>> Handle(ProductsByDescriptionQuery request, CancellationToken cancellationToken)
    {
        var products = await productMongoContext.GetAsync(request.Description);
        return from p in products
               select p.ToProductQueryResponse();
    }
}

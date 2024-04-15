using MediatR;
using Product.Api.Domain.Queries;
using Product.Api.Domain.Responses;
using Product.Api.Infrastructure.Data.ReadOnly;

namespace Product.Api.Domain.QueryHandlers;

public sealed class ProductByIdQueryHandler(IProductMongoContext productMongoContext) 
    : IRequestHandler<ProductByIdQuery, ProductQueryResponse?>
{
    public async Task<ProductQueryResponse?> Handle(ProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await productMongoContext.GetAsync(request.Id);
        return product?.ToProductQueryResponse();
    }
}

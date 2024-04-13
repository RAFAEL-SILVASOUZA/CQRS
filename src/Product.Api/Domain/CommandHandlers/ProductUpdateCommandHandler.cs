using MediatR;
using Product.Api.Domain.Command;
using Product.Api.Domain.Entities;
using Product.Api.Domain.Events;
using Product.Api.Domain.Responses;
using Product.Api.Infrastructure;

namespace Product.Api.Domain.CommandHandlers;

public class ProductUpdateCommandHandler(ProductDbContext productDbContext, IMediator mediator)
    : IRequestHandler<ProductUpdateCommand, ProductUpdatedResponse>
{
    public async Task<ProductUpdatedResponse> Handle(ProductUpdateCommand request, CancellationToken cancellationToken)
    {
        var product = await productDbContext.Products.FindAsync(request.Id) ?? throw new ArgumentNullException(nameof(ProductUpdateCommand));
        product = request.ChangeProduct(product);

        productDbContext.Update(product);
        await productDbContext.SaveChangesAsync(cancellationToken);
        await mediator.Publish(new ProductUpdatedEvent(request.Id,product.ToBsonProduct()), cancellationToken);

        return product.ToProductUpdatedResponse();
    }
}

using MediatR;
using Product.Api.Domain.Command;
using Product.Api.Domain.Events;
using Product.Api.Infrastructure;

namespace Product.Api.Domain.CommandHandlers;

public class ProductDeleteComandHandler(ProductDbContext productDbContext, IMediator mediator)
    : IRequestHandler<ProductDeleteCommand, Unit>
{
    public async Task<Unit> Handle(ProductDeleteCommand request, CancellationToken cancellationToken)
    {
        var product = await productDbContext.Products.FindAsync(request.Id) ?? throw new ArgumentNullException(nameof(ProductUpdateCommand));

        productDbContext.Products.Remove(product);
        await productDbContext.SaveChangesAsync(cancellationToken);
        await mediator.Publish(new ProductDeletedEvent(request.Id), cancellationToken);

        return Unit.Value;
    }
}

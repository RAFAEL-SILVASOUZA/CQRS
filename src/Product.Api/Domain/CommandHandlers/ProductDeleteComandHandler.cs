using MediatR;
using Product.Api.Domain.Command;
using Product.Api.Domain.Constants;
using Product.Api.Domain.Events;
using Product.Api.Domain.Notifications;
using Product.Api.Infrastructure.Data;

namespace Product.Api.Domain.CommandHandlers;

public class ProductDeleteComandHandler(IDomainNotificationContext domainNotificationContext,
                                        ProductDbContext productDbContext, 
                                        IMediator mediator)
    : IRequestHandler<ProductDeleteCommand, Unit>
{
    public async Task<Unit> Handle(ProductDeleteCommand request, CancellationToken cancellationToken)
    {
        var product = await productDbContext.Products.FindAsync(request.Id);

        if (product is null)
        {
            domainNotificationContext.NotifyNotFound(Errors.PRODUCT_NOT_FOUND);
            return Unit.Value;
        }

        productDbContext.Products.Remove(product);
        await productDbContext.SaveChangesAsync(cancellationToken);
        await mediator.Publish(new ProductDeletedEvent(request.Id), cancellationToken);

        return Unit.Value;
    }
}

using MediatR;
using Product.Api.Domain.Command;
using Product.Api.Domain.Events;
using Product.Api.Domain.Notifications;
using Product.Api.Infrastructure.Data;

namespace Product.Api.Domain.CommandHandlers;

public class ProductDeleteComandHandler(IDomainNotificationContext domainNotificationContext,
                                        ProductDbContext productDbContext,
                                        ILogger<ProductDeleteComandHandler> logger,
                                        IMediator mediator)
    : IRequestHandler<ProductDeleteCommand, Unit>
{
    public async Task<Unit> Handle(ProductDeleteCommand request, CancellationToken cancellationToken)
    {
        var product = await productDbContext.Products.FindAsync(request.Id);

        try
        {
            productDbContext.Products.Remove(product);
            await productDbContext.SaveChangesAsync(cancellationToken);
            await mediator.Publish(new ProductDeletedEvent(request.Id), cancellationToken);
            logger.LogInformation("Product removed.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error on remove product");
            domainNotificationContext.NotifyException(ex.Message);
        }

        return Unit.Value;
    }
}

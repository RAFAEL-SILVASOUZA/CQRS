using MediatR;
using Product.Api.Domain.Command;
using Product.Api.Domain.Commands.CommandExtensions;
using Product.Api.Domain.Entities;
using Product.Api.Domain.Events;
using Product.Api.Domain.Notifications;
using Product.Api.Domain.Responses;
using Product.Api.Infrastructure.Data;

namespace Product.Api.Domain.CommandHandlers;

public class ProductUpdateCommandHandler(IDomainNotificationContext domainNotificationContext,
                                         ProductDbContext productDbContext,
                                         ILogger<ProductUpdateCommandHandler> logger,
                                         IMediator mediator)
    : IRequestHandler<ProductUpdateCommand, ProductUpdatedResponse?>
{
    public async Task<ProductUpdatedResponse?> Handle(ProductUpdateCommand request, CancellationToken cancellationToken)
    {
        var product = await productDbContext.Products.FindAsync(request.Id);
        product = request.ChangeProduct(product);

        try
        {
            productDbContext.Update(product);
            await productDbContext.SaveChangesAsync(cancellationToken);
            await mediator.Publish(new ProductUpdatedEvent(request.Id, product.ToBsonProduct()), cancellationToken);
            logger.LogInformation("Product updated.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error on update product");
            domainNotificationContext.NotifyException(ex.Message);
        }

        return product.ToProductUpdatedResponse();
    }
}

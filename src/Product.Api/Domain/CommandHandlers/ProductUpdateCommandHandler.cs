using MediatR;
using Product.Api.Domain.Command;
using Product.Api.Domain.Commands.CommandExtensions;
using Product.Api.Domain.Constants;
using Product.Api.Domain.Entities;
using Product.Api.Domain.Events;
using Product.Api.Domain.Notifications;
using Product.Api.Domain.Responses;
using Product.Api.Infrastructure.Data;

namespace Product.Api.Domain.CommandHandlers;

public class ProductUpdateCommandHandler(IDomainNotificationContext domainNotificationContext,
                                         ProductDbContext productDbContext, 
                                         IMediator mediator)
    : IRequestHandler<ProductUpdateCommand, ProductUpdatedResponse>
{
    public async Task<ProductUpdatedResponse?> Handle(ProductUpdateCommand request, CancellationToken cancellationToken)
    {
        var product = await productDbContext.Products.FindAsync(request.Id);

        if (product is null)
        {
            domainNotificationContext.NotifyNotFound(Errors.PRODUCT_NOT_FOUND);
            return default;
        }

        product = request.ChangeProduct(product);

        productDbContext.Update(product);
        await productDbContext.SaveChangesAsync(cancellationToken);
        await mediator.Publish(new ProductUpdatedEvent(request.Id,product.ToBsonProduct()), cancellationToken);

        return product.ToProductUpdatedResponse();
    }
}

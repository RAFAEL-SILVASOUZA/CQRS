using MediatR;
using Microsoft.Extensions.Logging;
using Product.Api.Domain.Command;
using Product.Api.Domain.Commands.CommandExtensions;
using Product.Api.Domain.Entities;
using Product.Api.Domain.Events;
using Product.Api.Domain.Notifications;
using Product.Api.Domain.Responses;
using Product.Api.Infrastructure.Data;

namespace Product.Api.Domain.CommandHandlers;

public sealed class ProductCreateCommandHandler(IDomainNotificationContext domainNotificationContext,
                                                ProductDbContext productDbContext,
                                                ILogger<ProductCreateCommandHandler> logger,
                                                IMediator mediator)
    : IRequestHandler<ProductCreateCommand, ProductCreatedResponse>
{
    public async Task<ProductCreatedResponse> Handle(ProductCreateCommand request, CancellationToken cancellationToken)
    {
        var product = request.ToProduct();

        try
        {
            await productDbContext.Products.AddAsync(product);
            await productDbContext.SaveChangesAsync(cancellationToken);
            await mediator.Publish(new ProductCreatedEvent(product.ToBsonProduct()), cancellationToken);
            logger.LogInformation("Product created.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error on create product");
            domainNotificationContext.NotifyException(ex.Message);
        }

        return product.ToProductCreatedResponse();
    }
}

using MediatR;
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
        }
        catch (Exception ex)
        {
            domainNotificationContext.NotifyException(ex.Message);
        }

        return product.ToProductCreatedResponse();
    }
}

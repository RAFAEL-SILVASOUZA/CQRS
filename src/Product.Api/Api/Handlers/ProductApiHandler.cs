using MediatR;
using Product.Api.Domain.Command;
using Product.Api.Domain.Commands.Validators;
using Product.Api.Domain.Notifications;
using Product.Api.Domain.Queries;
using Product.Api.Infrastructure.Data;
using Product.Api.Infrastructure.Data.ReadOnly;

namespace Product.Api.Api.Handlers;

public sealed class ProductApiHandler(IMediator mediat,
                                      ProductDbContext productDbContext,
                                      IProductMongoContext productMongoContext,
                                      IDomainNotificationContext domainNotificationContext)
{
    public async Task<IResult> Post(ProductCreateCommand productCreateCommand)
    {
        var productCreated = await mediat.Send(productCreateCommand);

        if (domainNotificationContext.HasErrorNotifications)
            return domainNotificationContext.GetErrorNotifications();

        return Results.Created(string.Empty,productCreated);
    }

    public async Task<IResult> Put(ProductUpdateCommand productUpdateCommand)
    {
        var productUpdated = await mediat.Send(productUpdateCommand);

        if (domainNotificationContext.HasErrorNotifications)
            return domainNotificationContext.GetErrorNotifications();

        return Results.Ok(productUpdated);
    }

    public async Task<IResult> Delete(Guid id)
    {
        var command = new ProductDeleteCommand(id);
        var validator = new ProductDeleteCommandValidatior(productDbContext, productMongoContext, domainNotificationContext);
        _ = await validator.ValidateAsync(command);

        if (domainNotificationContext.HasErrorNotifications)
            return domainNotificationContext.GetErrorNotifications();

        await mediat.Send(new ProductDeleteCommand(id));

        if (domainNotificationContext.HasErrorNotifications)
            return domainNotificationContext.GetErrorNotifications();

        return Results.NoContent();
    }

    public async Task<IResult> Get()
    {
        var products = await mediat.Send(new ProductsQuery());
        return products.Count() > 0 ? Results.Ok(products) : Results.NoContent();
    }

    public async Task<IResult> Get(Guid id)
    {
        var product = await mediat.Send(new ProductByIdQuery(id));
        return product is not null ? Results.Ok(product) : Results.NotFound();
    }

    public async Task<IResult> Get(string filter)
    {
        var products = await mediat.Send(new ProductsByDescriptionQuery(filter));
        return products.Count() > 0 ? Results.Ok(products) : Results.NotFound();
    }
}

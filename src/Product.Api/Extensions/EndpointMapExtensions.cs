using MediatR;
using Microsoft.AspNetCore.Mvc;
using Product.Api.Domain.Command;
using Product.Api.Domain.Commands.Validators;
using Product.Api.Domain.Notifications;
using Product.Api.Domain.Queries;
using Product.Api.Domain.Responses;
using Product.Api.Infrastructure.Data;

namespace Product.Api.Extensions;

public static class EndpointMapExtensions
{
    public static RouteGroupBuilder MapProductApi(this RouteGroupBuilder group)
    {
        group.MapPost("", async ([FromServices] IMediator mediat,
            [FromServices] IDomainNotificationContext domainNotificationContext,
            [FromBody] ProductCreateCommand productCreateCommand) =>
        {
            var p = await mediat.Send(productCreateCommand);

            if (domainNotificationContext.HasErrorNotifications)
                return domainNotificationContext.GetErrorNotifications();

            return Results.Ok(p);
        })
        .WithName("Create Product")
        .Produces<ProductCreatedResponse>()
        .WithOpenApi();

        group.MapPut("", async ([FromServices] IMediator mediat,
            [FromServices] IDomainNotificationContext domainNotificationContext,
            [FromBody] ProductUpdateCommand productUpdateCommand) =>
        {
            var p = await mediat.Send(productUpdateCommand);

            if (domainNotificationContext.HasErrorNotifications)
                return domainNotificationContext.GetErrorNotifications();

            return Results.Ok(p);
        })
        .WithName("Update Product")
        .Produces<ProductUpdatedResponse>()
        .WithOpenApi();

        group.MapDelete("/{id}", async ([FromServices] IMediator mediat,
            [FromServices] ProductDbContext productDbContext,
            [FromServices] IDomainNotificationContext domainNotificationContext,
            [FromRoute(Name = "Id")] Guid id) =>
        {
            var command = new ProductDeleteCommand(id);
            var validator = new ProductDeleteCommandValidatior(productDbContext, domainNotificationContext);
            _ = await validator.ValidateAsync(command);

            if (domainNotificationContext.HasErrorNotifications)
                return domainNotificationContext.GetErrorNotifications();

            await mediat.Send(new ProductDeleteCommand(id));

            if (domainNotificationContext.HasErrorNotifications)
                return domainNotificationContext.GetErrorNotifications();

            return Results.NoContent();
        })
        .WithName("Delete Product")
        .WithOpenApi();


        group.MapGet("", async ([FromServices] IMediator mediat) =>
        {
            var product = await mediat.Send(new ProductsQuery());
            return Results.Ok(product);
        })
        .WithName("Get All Products")
        .Produces<IEnumerable<ProductQueryResponse>>()
        .WithOpenApi();

        group.MapGet("/{id}", async ([FromServices] IMediator mediat, [FromRoute] Guid id) =>
        {
            var product = await mediat.Send(new ProductByIdQuery(id));
            return Results.Ok(product);
        })
        .WithName("Get Product By Id")
        .Produces<ProductQueryResponse>()
        .WithOpenApi();

        group.MapGet("/filter", async ([FromServices] IMediator mediat, [FromQuery] string filter) =>
        {
            var products = await mediat.Send(new ProductsByDescriptionQuery(filter));
            return Results.Ok(products);
        })
        .WithName("Get Product By Description")
        .Produces<IEnumerable<ProductQueryResponse>>()
        .WithOpenApi();

        return group;
    }
}

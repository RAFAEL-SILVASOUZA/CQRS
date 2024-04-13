using MediatR;
using Microsoft.AspNetCore.Mvc;
using Product.Api.Domain.Command;
using Product.Api.Domain.Queries;
using Product.Api.Domain.Responses;

namespace Product.Api.Extensions
{
    public static class EndpointMapExtensions
    {
        public static RouteGroupBuilder MapProductApi(this RouteGroupBuilder group)
        {
            group.MapPost("/create", async ([FromServices] IMediator mediat, [FromBody] ProductCreateCommand productCreateCommand) =>
            {
                var p = await mediat.Send(productCreateCommand);
                return Results.Ok(p);
            })
            .WithName("Create Product")
            .Produces<ProductCreatedResponse>()
            .WithOpenApi();

            group.MapPut("/update/{Id}", async ([FromServices] IMediator mediat, [FromBody] ProductUpdateCommand productUpdateCommand) =>
            {
                var p = await mediat.Send(productUpdateCommand);
                return Results.Ok(p);
            })
            .WithName("Update Product")
            .Produces<ProductUpdatedResponse>()
            .WithOpenApi();

            group.MapDelete("/delete/{id}", async ([FromServices] IMediator mediat, [FromRoute] Guid id) =>
            {
                await mediat.Send(new ProductDeleteCommand(id));
                return Results.NoContent();
            })
            .WithName("Delete Product")
            .WithOpenApi();


            group.MapGet("/all", async ([FromServices] IMediator mediat) =>
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

            group.MapGet("/filter", async ([FromServices] IMediator mediat, [FromQuery] string description) =>
            {
                var products = await mediat.Send(new ProductsByDescriptionQuery(description));
                return Results.Ok(products);
            })
            .WithName("Get Product By Description")
            .Produces<IEnumerable<ProductQueryResponse>>()
            .WithOpenApi();

            return group;
        }
    }
}

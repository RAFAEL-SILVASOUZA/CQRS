using Microsoft.AspNetCore.Mvc;
using Product.Api.Api.Handlers;
using Product.Api.Domain.Command;
using Product.Api.Domain.Responses;

namespace Product.Api.Extensions;

public static class EndpointMapExtensions
{
    public static RouteGroupBuilder MapProductApi(this RouteGroupBuilder group)
    {
        group.MapPost("", ([FromBody] ProductCreateCommand productCreateCommand, [FromServices] ProductApiHandler productApiHandler)
              => productApiHandler.Post(productCreateCommand))
             .WithName("Create Product")
             .Produces<ProductCreatedResponse>()
             .WithOpenApi();

        group.MapPut("", ([FromBody] ProductUpdateCommand productUpdateCommand, [FromServices] ProductApiHandler productApiHandler)
              => productApiHandler.Put(productUpdateCommand))
             .WithName("Update Product")
             .Produces<ProductUpdatedResponse>()
             .WithOpenApi();

        group.MapDelete("/{id}", ([FromRoute(Name = "Id")] Guid id, [FromServices] ProductApiHandler productApiHandler)
              => productApiHandler.Delete(id))
             .WithName("Delete Product")
             .WithOpenApi();

        group.MapGet("", ([FromServices] ProductApiHandler productApiHandler)
              => productApiHandler.Get())
             .WithName("Get All Products")
             .Produces<IEnumerable<ProductQueryResponse>>()
             .WithOpenApi();

        group.MapGet("/{id}", ([FromRoute] Guid id, [FromServices] ProductApiHandler productApiHandler)
              => productApiHandler.Get(id))
             .WithName("Get Product By Id")
             .Produces<ProductQueryResponse>()
             .WithOpenApi();


        group.MapGet("/filter", ([FromQuery] string filter, [FromServices] ProductApiHandler productApiHandler)
              => productApiHandler.Get(filter))
             .WithName("Get Product By Description")
             .Produces<IEnumerable<ProductQueryResponse>>()
             .WithOpenApi();

        return group;
    }
}

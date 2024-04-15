namespace Product.Api.Domain.Responses;

public record ProductQueryResponse(Guid Id, string Description, double Price) : IResponse;


public static class ProductQueryResponseFactoryExtension
{
    public static ProductQueryResponse ToProductQueryResponse(this Entities.BsonProduct product)
    => new ProductQueryResponse(new Guid(product.Id), product.Description, product.Price);
}